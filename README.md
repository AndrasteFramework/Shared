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