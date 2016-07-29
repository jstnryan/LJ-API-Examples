Overview
---
These are example applications intended to demonstrate communicating with and controlling [Martin LightJockey](http://www.martin.com/en-us/product-details/lightjockey-2) from an external application. Most examples were inspired by the original Borland Delphi [code samples](https://martinprofessional.app.box.com/v/lightjockey/1/1721411436) provided by Martin staff and LightJockey enthusiasts (included where available).

Projects
---
  * [LJ-ExternalDMXOverride](/LJ-ExternalDMXOverride) - Demonstrates the use of SendMessage function with WM_COPYDATA to send raw DMX values directly to LightJockey's output, overwriting LJ's own values. Delphi5, and VB.NET (Visual Studio 2008, 2015) projects included.
  * [LJ-ExternalIntensityControl](/LJ-ExternalIntensityControl) - Demonstrates the use of SendMessage function with WM_COPYDATA to control master intensity and group-master intensities. Delphi5, and VB.NET (Visual Studio 2008, 2015) projects included.

License (MIT License)
---
Copyright (c) 2016 Justin Ryan

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.