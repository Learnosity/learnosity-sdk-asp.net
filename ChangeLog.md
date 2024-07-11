# Change Log

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [v0.10.0] - 2024-07-11
### Added

- Add support for Authoraide API.

## [v0.9.3] - 2024-05-02

### Fixed

- Handle Null Reference Exception raised under load, [see for more details](https://github.com/Learnosity/learnosity-sdk-asp.net/pull/47)

### Security

- Bump Newtonsoft.JSON from 10.0.3 to 13.0.2

## [v0.9.2] - 2023-06-29
### Security
- Upgraded signature to match the security standard.

## [v0.9.1] - 2019-12-23
### Added

- More unit tests that cover the `Init` class.

### Fixed

- Fixed a bug where `null` and empty string request packets would cause an exception to be thrown when calling the `Init` constructor.

## [v0.9.0] - 2019-08-12
### Added

- This ChangeLog!
- Telemetry data (basic information about the execution environment) is now added to the request objects being signed which is later read and logged internally by our APIs when the request is received. This allows us to better support our various SDKs and does not send any additional network requests. More information can be found in README.md.
