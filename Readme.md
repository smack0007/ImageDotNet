[![The MIT License](https://img.shields.io/badge/license-MIT-orange.svg?style=flat-square)](http://opensource.org/licenses/MIT)
[![Actions Status](https://github.com/smack0007/ImageDotNet/workflows/CI/badge.svg)](https://github.com/smack0007/ImageDotNet/actions)
[![NuGet Badge](https://buildstats.info/nuget/ImageDotNet)](https://www.nuget.org/packages/ImageDotNet/)

# ImageDotNet

A library for loading and saving images in .NET.

## Goals

The goal of the library is to serve a purpose similar to [stb_image](https://github.com/nothings/stb/blob/master/stb_image.h) and
[stb_image_write](https://github.com/nothings/stb/blob/master/stb_image_write.h) except written in C#. The goals include:

* Loading images.
* Writing images.
* Converting between different pixel formats.

## Example

```C#
// Load image.png and ensure the pixel format is Rgba32.
var image = Image.LoadPng("image.png").To<Rgba32>();

// Get a pointer to the image data and pass it to glTexImage2D.
using (var data = image.GetDataPointer())
{
	glTexImage2D(GL_TEXTURE_2D, 0, (int)GL_RGBA, image.Width, image.Height, 0, GL_RGBA, GL_UNSIGNED_BYTE, (void*)data.Pointer);
}
```

## Credits

Thanks to:
* [ImageSharp](https://github.com/Zulkir/ImageSharp) from [Zulkir](https://github.com/Zulkir) for helping me to understand some parts of PNG decoding.
* [ImageSharp](https://github.com/SixLabors/ImageSharp) from [Six Labors](https://github.com/SixLabors). I took the names for the pixel formats from there.
