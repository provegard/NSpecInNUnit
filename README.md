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

    public class my_examples : nspec_as_nunit<my_examples> {

      public void describe_the_examples() {
        it["should work"] = () => Assert.That(1, Is.EqualTo(2));
      }

    }

Note that the base class is a generic class that has to be parameterized with the examples
class. This is required because of how NUnit 3 works.

Also note that NSpec no longer ships with its own assertions (other than a few limited). You can use NUnit 
assertions like above or install [another assertion library](http://nspec.org/#assertions).

For more information about NSpec, please visit http://nspec.org/.

## Limitations

* Only works with NUnit 3.
* For NUnit 2 support, please use NSpecInNUnit 2.0.1.
* For NSpec 1 support, please use NSpecInNUnit 1.0.1.
* All examples are run together. Running a single test (e.g. via ReSharper) only affects
  the test reporting.

## Changelog

### 3.0.0.0 (2017-02-23)

* Now depends on NUnit 3.6.0.

### 2.0.1.0 (2017-02-23)

* Remove unnecessary NUnit references.

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
