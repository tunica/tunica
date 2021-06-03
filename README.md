# tunica

Tunica is a .NET wrapper for [askalono](https://github.com/amzn/askalono), a library and command-line tool to help detect license texts.

## Usage

Tunica currently supports the `crawl` action, which discovers licenses in a specified directory:

```csharp
using (var checker = new Tunica.Checker())
{
    var licenses = new checker.Crawl("C:\src");
    
    // Do something with the discovered licenses
}
```

## Name

askalono means "shallot" in Esperanto, and tunica is the membrane that *wraps* around an onion.
