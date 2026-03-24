# Security Policy

## Supported Versions

| Version | Supported          |
|---------|--------------------|
| 1.x     | :white_check_mark: |

## Reporting a Vulnerability

If you discover a security vulnerability in CoreFX SDK, please report it responsibly.

**Do NOT open a public GitHub issue for security vulnerabilities.**

Instead, please send an email to the maintainer or use [GitHub Security Advisories](https://github.com/osisdie/dotnet-corefx-sdk/security/advisories/new) to report the vulnerability privately.

### What to Include

- Description of the vulnerability
- Steps to reproduce
- Affected module(s) and version(s)
- Potential impact
- Suggested fix (if any)

### Response Timeline

- **Acknowledgment**: Within 48 hours
- **Initial assessment**: Within 1 week
- **Fix and release**: Based on severity, typically within 2 weeks for critical issues

### Security Considerations

This SDK handles security-sensitive operations including:

- **JWT Authentication** (`CoreFX.Auth`) — Token generation, validation, and refresh with HS256 signing
- **SMTP Credentials** (`CoreFX.Notification`) — Email service with credential management via environment variables
- **TLS Configuration** (`CoreFX.Abstractions`) — SecurityProtocol settings in `SvcContext`

#### Best Practices When Using This SDK

- Store JWT secrets, API keys, and SMTP passwords in environment variables or a secret manager — never in source code or `appsettings.json`
- Use a strong JWT secret (at least 32 bytes)
- Keep token expiration times short (default: 60 minutes for access tokens)
- Rotate secrets regularly
- Use HTTPS in production
- Review middleware pipeline order — `ExceptionHandler_Middleware` should be registered first to catch all exceptions
