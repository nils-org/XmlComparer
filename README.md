# XmlComparer

[![standard-readme compliant][]][standard-readme]
[![All Contributors](https://img.shields.io/badge/all_contributors-0-orange.svg?style=flat-square)](#contributors)
[![Appveyor build][appveyorimage]][appveyor]
[![Codecov Report][codecovimage]][codecov]
[![NuGet package][nugetimage]][nuget]

> a library to compare xml documents (or nodes) and get a list of differences.

## Table of Contents

- [Status](#status)
- [Install](#install)
- [Usage](#usage)
- [Maintainer](#maintainer)
- [Contributing](#contributing)
  - [Contributors](#contributors)
- [License](#license)

## Status

I'll archive this project. I started it because I did not do enough research in the first place.

If someone comes here in need of a good XML comparer, please consider using https://github.com/BrutalSimplicity/XmlDiffLib

## Install

using nuget:

```ps
Install-Package XmlComparer
dotnet add XmlComparer
paket add XmlComparer
```

## Usage

See also the [local documentation][documentation] and [api][api]

### Adding files

```cs
var left = new XmlDocument();
left.Load("C:\temp\demo1.xml");

var right = new XmlDocument();
right.Load("C:\temp\demo2.xml");
            
var comparer = new XmlComparer.Comparer(
  ignoreAttributeOrder: true,
  ignoreNamespace: true)
var differences = comparer.GetDifferences(left, right);
```

## Maintainer

[Nils Andresen @nils-a][maintainer]

## Contributing

XmlComparer follows the [Contributor Covenant][contrib-covenant] Code of Conduct.

We accept Pull Requests.

Small note: If editing the Readme, please conform to the [standard-readme][] specification.

This project follows the [all-contributors][] specification. Contributions of any kind welcome!

### Contributors

Thanks goes to these wonderful people ([emoji key][emoji-key]):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore -->
<!-- ALL-CONTRIBUTORS-LIST:END -->

## License

[MIT License © Nils Andresen][license]

[all-contributors]: https://github.com/all-contributors/all-contributors
[appveyor]: https://ci.appveyor.com/project/nils-org/XmlComparer
[appveyorimage]: https://img.shields.io/appveyor/ci/nils-org/XmlComparer.svg?logo=appveyor&style=flat-square
[codecov]: https://codecov.io/gh/nils-org/XmlComparer
[codecovimage]: https://img.shields.io/codecov/c/github/nils-org/XmlComparer.svg?logo=codecov&style=flat-square
[contrib-covenant]: https://www.contributor-covenant.org/version/1/4/code-of-conduct
[emoji-key]: https://allcontributors.org/docs/en/emoji-key
[maintainer]: https://github.com/nils-a
[nuget]: https://nuget.org/packages/XmlComparer
[nugetimage]: https://img.shields.io/nuget/v/XmlComparer.svg?logo=nuget&style=flat-square
[license]: LICENSE.txt
[standard-readme]: https://github.com/RichardLitt/standard-readme
[standard-readme compliant]: https://img.shields.io/badge/readme%20style-standard-brightgreen.svg?style=flat-square
