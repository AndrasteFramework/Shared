# Andraste - The "native" C# Modding Framework

> Britain/Celtic goddess of war, symbolizing invincibility

The Andraste Modding Framework aims to be a solid base for those writing an
in-process modding framework for native (x86, 32bit) Windows applications (Games).

It is mostly the result of generalizing code that I would have written
specifically for one game. Releasing it may help others to quickly re-use
functionality as well as maybe contributing and reviewing decisions made here.

## The Shared Library
This repository holds the common base dependency that provides both generic 
API/Utility to be used throughout Andraste as well as hosting the data types /
json models that are used for communication between the Host and the Payload
libraries.

## Binding Redirects
Every dependency added here may ultimately require an App.config in the mod framework.
This is because plugins may be compiled against older versions of this dependency
(or not updating their dependencies), as a result we need an App.config file.

My best experience so far in fighting problems after dependency upgrades was to
follow the manual procedure outlined [here](https://nickcraver.com/blog/2020/02/11/binding-redirects/)
Apparently using the "old" `packages.config` file, one can also use the
`Get-Project –All | Add-BindingRedirect` command to generate those files, so
maybe a valid strategy is to keep a dummy project with `packages.config` around,
generate the file and copy it over from time to time...