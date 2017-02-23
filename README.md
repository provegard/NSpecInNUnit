# NSpecInNUnit

A repo to demonstrate running NSpec examples in a NUnit test suite.

One purpose is to help demonstrate this issue: https://github.com/mattflo/NSpec/issues/114

## How to use

Install using Package Manager Console in Visual Studio:

    Install-Package NSpecInNUnit

Or if you prefer a UI, find it via the Manage NuGet Packages for Solution menu.

Once installed, it's a matter of using the base class:

    using NSpec;
    using NSpecInNUnit;

    public class my_examples : nspec_as_nunit {

      public void describe_the_examples() {
        it["should work"] = () => 1.should_be(1);
      }

    }

For more information about NSpec, please visit http://nspec.org/.

## Limitations

* Only works with NUnit 2
* All examples are run together. Running a single test (e.g. via ReSharper) only affects
  the test reporting.

## Changelog

### 2.0.0.0 (2017-02-23)

* Target .NET 4.5.2.
* Use NSpec 2.1.0.

### 1.0.1.0 (2017-02-23)

* Target .NET 4.5, because one reason to stick to NSpec 1 is because you need to
  use .NET 4.5 or 4.5.1, so it makes sense that NSpecInNUnit doesn't have a
  requirement on a newer version.

### 1.0.0.0 (2017-02-06)

* First version, works with NSpec 1 and NUnit 2.

## License

The code is licensed under the MIT license. See https://per.mit-license.org/2016
