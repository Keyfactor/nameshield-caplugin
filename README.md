<h1 align="center" style="border-bottom: none">
    Nameshield   Gateway AnyCA Gateway REST Plugin
</h1>

<p align="center">
  <!-- Badges -->
<img src="https://img.shields.io/badge/integration_status-prototype-3D1973?style=flat-square" alt="Integration Status: prototype" />
<a href="https://github.com/Keyfactor/nameshield-caplugin/releases"><img src="https://img.shields.io/github/v/release/Keyfactor/nameshield-caplugin?style=flat-square" alt="Release" /></a>
<img src="https://img.shields.io/github/issues/Keyfactor/nameshield-caplugin?style=flat-square" alt="Issues" />
<img src="https://img.shields.io/github/downloads/Keyfactor/nameshield-caplugin/total?style=flat-square&label=downloads&color=28B905" alt="GitHub Downloads (all assets, all releases)" />
</p>

<p align="center">
  <!-- TOC -->
  <a href="#support">
    <b>Support</b>
  </a> 
  ·
  <a href="#requirements">
    <b>Requirements</b>
  </a>
  ·
  <a href="#installation">
    <b>Installation</b>
  </a>
  ·
  <a href="#license">
    <b>License</b>
  </a>
  ·
  <a href="https://github.com/orgs/Keyfactor/repositories?q=anycagateway">
    <b>Related Integrations</b>
  </a>
</p>


The Nameshield AnyCA Gateway REST plugin extends the capabilities of Nameshield CA to Keyfactor Command via the Keyfactor AnyCA Gateway REST. The plugin represents a fully featured AnyCA REST Plugin with the following capabilies:
* SSL Certificate Synchronization
    * All Sync jobs are currently treated as a full sync
    * Certificates will only syncronize once unless their status changes.  If a certificate is found based on Serial Number for the managed CA, and its status is unchanged, it will be skipped for subsequent syncs to minimize impact on Cert Manager API load
* SSL Certificate Enrollment
* SSL Certificate Revocation

## Compatibility

The Nameshield   Gateway AnyCA Gateway REST plugin is compatible with the Keyfactor AnyCA Gateway REST 24.2.0 and later.

## Support
The Nameshield   Gateway AnyCA Gateway REST plugin is open source and community supported, meaning that there is **no SLA** applicable. 

> To report a problem or suggest a new feature, use the **[Issues](../../issues)** tab. If you want to contribute actual bug fixes or proposed enhancements, use the **[Pull requests](../../pulls)** tab.

## Requirements

TODO Requirements is a required section

## Installation

1. Install the AnyCA Gateway REST per the [official Keyfactor documentation](https://software.keyfactor.com/Guides/AnyCAGatewayREST/Content/AnyCAGatewayREST/InstallIntroduction.htm).

2. On the server hosting the AnyCA Gateway REST, download and unzip the latest [Nameshield   Gateway AnyCA Gateway REST plugin](https://github.com/Keyfactor/nameshield-caplugin/releases/latest) from GitHub.

3. Copy the unzipped directory (usually called `net6.0`) to the Extensions directory:

    ```shell
    Program Files\Keyfactor\AnyCA Gateway\AnyGatewayREST\net6.0\Extensions
    ```

    > The directory containing the Nameshield   Gateway AnyCA Gateway REST plugin DLLs (`net6.0`) can be named anything, as long as it is unique within the `Extensions` directory.

4. Restart the AnyCA Gateway REST service.

5. Navigate to the AnyCA Gateway REST portal and verify that the Gateway recognizes the Nameshield   Gateway plugin by hovering over the ⓘ symbol to the right of the Gateway on the top left of the portal.

## Configuration

1. Follow the [official AnyCA Gateway REST documentation](https://software.keyfactor.com/Guides/AnyCAGatewayREST/Content/AnyCAGatewayREST/AddCA-Gateway.htm) to define a new Certificate Authority, and use the notes below to configure the **Gateway Registration** and **CA Connection** tabs:

    * **Gateway Registration**

        In order to enroll for certificates the Keyfactor Command server must trust the trust chain. Once you set your Root and/or Subordinate CA in your Nameshield account, make sure to download and import the certificate chain into the Command Server certificate store

    * **CA Connection**

        Populate using the configuration fields collected in the [requirements](#requirements) section.

        * **ApiUrl** - The base URL to send API requests to. Standard URLs are https://api.nameshield.net/ for production environment, and https://ote-api.nameshield.net/ for test and development 
        * **ApiToken** - The Bearer token to use to authenticate to the Nameshield API 
        * **Enabled** - Flag to Enable or Disable gateway functionality. Diabling is primarily used to allow creation of the CA prior to configuration information being available. 

2. When defining templates, use the product names as the ProductID (e.g. Digicert Secure Site EV) as opposed to the ID (e.g. ABC012)

3. Follow the [official Keyfactor documentation](https://software.keyfactor.com/Guides/AnyCAGatewayREST/Content/AnyCAGatewayREST/AddCA-Keyfactor.htm) to add each defined Certificate Authority to Keyfactor Command and import the newly defined Certificate Templates.

4. In Keyfactor Command (v12.3+), for each imported Certificate Template, follow the [official documentation](https://software.keyfactor.com/Core-OnPrem/Current/Content/ReferenceGuide/Configuring%20Template%20Options.htm) to define enrollment fields for each of the following parameters:

    * **OrganizationId** - If you know your organization's ID in nameshield, you can supply that here and that will be used as the organization regardless of what values exist in the request or the Organization field 
    * **Organization** - If OrganizationId is empty, and an organization name is provided here, the Nameshield gateway will use that name to do organization lookups when enrolling. If neither OrganizationId nor Organization is supplied, the gateway will use whatever is in the O= field of the request subject. 



## License

Apache License 2.0, see [LICENSE](LICENSE).

## Related Integrations

See all [Keyfactor Any CA Gateways (REST)](https://github.com/orgs/Keyfactor/repositories?q=anycagateway).