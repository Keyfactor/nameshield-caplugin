## Overview

The Nameshield AnyCA Gateway REST plugin extends the capabilities of Nameshield CA to Keyfactor Command via the Keyfactor AnyCA Gateway REST. The plugin represents a fully featured AnyCA REST Plugin with the following capabilies:
* SSL Certificate Synchronization
    * All Sync jobs are currently treated as a full sync
    * Certificates will only syncronize once unless their status changes.  If a certificate is found based on Serial Number for the managed CA, and its status is unchanged, it will be skipped for subsequent syncs to minimize impact on Cert Manager API load
* SSL Certificate Enrollment
* SSL Certificate Revocation

## Gateway Registration

In order to enroll for certificates the Keyfactor Command server must trust the trust chain. Once you set your Root and/or Subordinate CA in your Nameshield account, make sure to download and import the certificate chain into the Command Server certificate store

## Certificate Template Creation Step

When defining templates, use the product names as the ProductID (e.g. Digicert Secure Site EV) as opposed to the ID (e.g. ABC012)

## Requirements

TODO Requirements is a required section

