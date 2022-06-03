# Base64Stream

Stream for base64 encoding/decoding

# Examples

### Code

```CSharp
var original = "Hello, World!\r\n안녕하세요!";
byte[] encoded = null;

using (var output = new MemoryStream())
{
	using (var bs = new Base64Stream(output, true))
	{
		bs.Write(Encoding.Default.GetBytes(original));
	}

	encoded = output.ToArray();
}

string decoded = null;

using (var input = new MemoryStream(encoded))
{
	using (var bs = new Base64Stream(input, true))
	{
		using (var temp = new MemoryStream())
		{
			bs.CopyTo(temp);
			decoded = Encoding.Default.GetString(temp.ToArray());
		}

	}

}
```

### Console

```CSharp
===== Original =====
Hello, World!
안녕하세요!

===== Encoded =====
SGVsbG8sIFdvcmxkIQ0K7JWI64WV7ZWY7IS47JqUIQ==

===== Decoded =====
Hello, World!
안녕하세요!

===== Test Result =====
True
```