using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using Duende.IdentityServer;
using Duende.IdentityServer.Stores;
using Microsoft.Extensions.Options;
using Sustainsys.Saml2;
using Sustainsys.Saml2.AspNetCore2;
using Sustainsys.Saml2.Metadata;
using Xceleration.RSBusiness.IdentityServer.Contracts.ExternalProviders;

namespace Xceleration.RSBusiness.IdentityServer.Domain.OptionConfiguration;

public class ConfigureSaml2Options : IConfigureNamedOptions<Saml2Options>
{
    private readonly IIdentityProviderStore _store;

    public ConfigureSaml2Options(IIdentityProviderStore store)
    {
        _store = store;
    }

    public void Configure(Saml2Options options)
    {
    }

    public void Configure(string name, Saml2Options options)
    {
        var provider = _store.GetBySchemeAsync(name).ConfigureAwait(false).GetAwaiter().GetResult();
        if (provider == null) return;
        var saml2Provider = new Saml2IdentityProvider(provider);
        SetupSaml2Connection(name, options, saml2Provider);
    }

    #region Private Methods

    private void SetupSaml2Connection(string schema, Saml2Options options, Saml2IdentityProvider saml2Provider)
    {
        options.SPOptions.ModulePath = $"/{saml2Provider.ModulePath.Trim()}";
        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        options.SignOutScheme = IdentityServerConstants.SignoutScheme;
        options.SPOptions.EntityId = new EntityId($"{saml2Provider.EntityId.Trim()}/{saml2Provider.ModulePath.Trim()}");
        if (!string.IsNullOrWhiteSpace(saml2Provider.MinIncomingSigningAlgorithm))
        {
            options.SPOptions.MinIncomingSigningAlgorithm = saml2Provider.MinIncomingSigningAlgorithm.Trim();
            if (saml2Provider.MinIncomingSigningAlgorithm.Trim() == SignedXml.XmlDsigRSASHA1Url)
                AddSha1AlgorithmToCrypt();
        }

        options.SPOptions.ServiceCertificates.Add(GenerateCertificate($"{schema}_SelfSigned")); // for Single logout.

        options.IdentityProviders.Add(
            new IdentityProvider(new EntityId(saml2Provider.IdentityProviderEntityId.Trim()),
                options.SPOptions)
            {
                MetadataLocation = saml2Provider.IdentityProviderMetadataLoc.Trim(),
                LoadMetadata = true
            });
        options.Notifications.ProcessSingleLogoutResponseStatus =
            (resp, state) => true; // ignore logout response from idp
    }

    private void AddSha1AlgorithmToCrypt()
    {
        try
        {
            // try to add the algorithm to crypt for logging out.
            CryptoConfig.AddAlgorithm(typeof(Rsapkcs1Sha1SignatureDescription),
                SignedXml.XmlDsigRSASHA1Url);
        }
        catch
        {
            // ignored
        }
    }

    private static X509Certificate2 GenerateCertificate(string subject)
    {
        using var rsa = RSA.Create(4096);
        var request = new CertificateRequest($"CN={subject}", rsa, HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);
        request.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
        request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));
        var cert = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddSeconds(-45),
            DateTimeOffset.UtcNow.AddYears(50));
        return cert;
    }

    #endregion Private Methods
}

/// <summary>
///     This is for Sha1 Hashing
/// </summary>
public class Rsapkcs1Sha1SignatureDescription : SignatureDescription
{
    private readonly string _hashAlgorithm;

    public Rsapkcs1Sha1SignatureDescription()
    {
        KeyAlgorithm = typeof(RSACryptoServiceProvider).FullName;
        DigestAlgorithm = typeof(SHA1).FullName;
        FormatterAlgorithm = typeof(RSAPKCS1SignatureFormatter).FullName;
        DeformatterAlgorithm = typeof(RSAPKCS1SignatureDeformatter).FullName;
        _hashAlgorithm = "SHA1";
    }

    public override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        var deformatted = new RSAPKCS1SignatureDeformatter(key);
        deformatted.SetHashAlgorithm(_hashAlgorithm);
        return deformatted;
    }

    public override AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        var formatter = new RSAPKCS1SignatureFormatter(key);
        formatter.SetHashAlgorithm(_hashAlgorithm);
        return formatter;
    }
}