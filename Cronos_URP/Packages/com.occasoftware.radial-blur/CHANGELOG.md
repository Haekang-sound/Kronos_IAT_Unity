# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/).
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

Changelog can be found online at [Radial Blur Changelog](https://www.occasoftware.com/changelogs/radial-blur).

## [3.0.0] - 2023-06-28
This version is compatible with Unity 2022.3.0f1.

### Changed
- Migrated to RTHandle and Blitter APIs
- Changed the shader graph to a custom shader


## [2.0.0] - 2023-06-12‍
This version is compatible with Unity 2021.3.0f1+.
### Changed
- Migrated to UPM-style package
- This asset is now compatible with the URP Volume Framework and uses that option instead of the Radial Blur Manager
- This asset now works better when loading scenes additively

### Removed
- The Radial Blur Manager has been removed and replaced with the Radial Blur Post Process.

## [1.1.0] - 2023-01-31
This version is compatible with Unity 2020.3.0f1+.
- Added a delay property, which allows you to delay the start of the blur effect from the center point.
- Backported to 2020.3.0

## [1.0.0]
- Initial release