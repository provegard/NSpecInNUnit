# NSpecInNUnit

A small adapter between NSpec and NUnit - allows you to run NSpec examples using an NUnit runner.
This is useful if your test infrastructure is based on NUnit but you want to migrate to NSpec, or
if you find that good tooling doesn't have NSpec support.

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
class (here `my_examples`). This is required because of how NUnit 3 test data sources works.

For more information about NSpec, please visit http://nspec.org/.

## Assertions

NSpec no longer ships with its own assertions (other than a few limited). You can use NUnit 
assertions like in the example above or install [another assertion library](http://nspec.org/#assertions).

&#x26a0; If you use a non-NUnit assertion library like FluentAssertions and have no NUnit Assert statements
at all, NUnit may not detect any tests in the assembly. To fix this, add the `[TestFixture]` attribute
to at least one of your example classes.

## Limitations

* Only works with NUnit 3.7 and later (due to [this bug](https://github.com/nunit/nunit/issues/2052)).
* Only works with NSpec 3.0.3 and later (due to .NET Core support)
* For NUnit 2 support, please use NSpecInNUnit 2.0.1.
* For NSpec 1 support, please use NSpecInNUnit 1.0.1.
* All examples are run together. Running a single test (e.g. via ReSharper) only affects
  the test reporting.

## .NET Core/Standard

NSpecInNUnit targets .NET Framework 4.5.2 as well as .NET Standard 1.6.
Thanks to David Vedvick (@danrien) for making the required changes!

## Changelog

### 3.1.0.0 (2017-11-12)

* Targets both .NET Framework 4.5.2 and .NET Standard 1.6.
* Now depends on NUnit 3.7.0.
* Now depends on NSpec 3.0.3.

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
