//
//  MoreString 1.51
//  Written by Tim "Zear" Hammock
//
//
//  The functions supplied here are as follows:
//
// String::left(%string, %len)
// Returns the leftmost %len characters from %string. For example
// 	    String::left("What is at the left?", 7)
// will return the string
// 	    "What is"
// 
// String::right(%string, %len)
// Returns the rightmost %len characters from %string. For example
// 	    String::right("What is at the right?", 7)
// will return the string
// 	    " right?"
// 
// String::starts(%string, %search)
// Returns true if the beginning of %string exactly (including case)
// matches %search, character for character, otherwise returns false. For
// example
//      String::starts("What is at the left?", "What")
// will return true, but
//      String::starts("What is at the left?", "what")
// will return false.
// 
// String::ends(%string, %search)
// Returns true if the end of %string exactly (including case) matches
// %search, character for character, otherwise returns false. For example
// 	    String::starts("What is at the right?", "right?")
// will return true, but
// 	    String::starts("What is at the right?", "right")
// will return false (no '?').
// 
// String::insert(%string, %insert, %idx)
// Returns a string where %insert has been inserted into %string and
// position %idx. For example
// 	    String::insert("Insert into this", "Tribes ", 7)
// will return the string
// 	    "Insert Tribes into this"
// 
// String::replace(%string, %search, %replace)
// Returns a string where all instances of %search in %string have been
// replaced with an instance of %replace. For example
// 	    String::replace("Insert into this", "Insert into", "Replace")
// will return the string
// 	    "Replace this"
// 
// String::setString(%chars, %num)
// Returns a string consiting of %num repititions of %chars. For example
// 	    String::setString("Four", 4)
// will return the string
// 	    "FourFourFourFour"
// 
// String::doubleSlashes(%string)
// Replaces all instances of "\" in %string with "\\" and returns the
// result. For example
// 	    String::doubleSlashes("config\autoexec.cs")
// will return the string
// 	    "config\\autoexec.cs"
// 
// String::halveSlashes(%string)
// Replaces all instances of "\\" in %string with "\" and returns the
// result. For example
// 	    String::halveSlashes("config\\autoexec.cs")
// will return the string
// 	    "config\autoexec.cs"
// 
// String::trim(%string)
// Returns %string after stripping off any leading or trailing spaces. For
// example
//      String::trim("  too many spaces   ")
// will return the string
//      "too many spaces"
// 
// String::pad(%string, %char, %width, %justify)
// Returns %string after padding it with enough %char's to give it a
// length of %width. If %char is more than one character in length, or if
// %width is less than the initial length of %string, then %string will be
// returned unmodified. Justify can be "left", "right", or "center"
// (actually only the first character of %justify is significant and case
// doesn't matter). For example
// 	    String::pad("XXX", "o", 10, "center")
// will return the string
// 	    "oooXXXoooo"
// The most common use will likely be one where %char is passed as " "
// (single space) to allow text to be lined up in columns.
// 
// String::indexOf(%string, %search, %idx)
// Returns an index indicating the position of %search in %string, much
// like the Dynamix function String::findSubStr(). The difference here is
// that indexOf() begins searching %string after %idx characters, allowing
// you to skip over instances of %search already found earlier in %string.
// For example
// 	    String::indexOf("A string that repeats. String repeats.", "repeats", 24)
// will return 30.
// 
// String::len(%string)
// Returns the length of %string in chars. For example
// 	    String::len("A string with length.")
// will return 21.
// 
// String::getWordCount(%string)
// Returns the number of words in %string, using spaces as delimiters. For
// example
// 	    String::getWordCount("A string of words to count.")
// will return 6. Note that the period at the end is considered a part of
// the word 'count' as there is no space between them.
// 
// String::ascii(%string, %idx)
// Returns the ascii value of the %idx'th character of %string. For example:
//      String::ascii("ABC", 0)
// will return 65. %idx is zero-based, so the first character is at %idx == 0.
// Only ascii numbers 32 through 126 are returned. If the character falls
// outside that range, if %idx < 0, or if %idx >= String::len(%string), a -1
// will be returned.
//
// String::char(%ascii)
// Returns the character having the ascii value %ascii. For example:
//      String::char(65)
// will return "A". Only ascii numbers 32 through 126 are returned. If the
// value falls outside that range a "" will be returned.
//
// String::pixels(%string)
// Returns the width of %string in pixels. Only ascii numbers 32 through 126 are
// considered. If the value falls outside that range its width will not be added.
//
// -------------------------------------------------
// 
// The Tribes-default string functions are:
//
// String::findSubStr(%string, %search)
// String::getSubStr(%string, %idx, %len)
// String::ICompare(%string, %search)
// String::NCompare(%string1, %string2, %len)
// String::empty(%string)
// getWord(%string, %idx)

function String::left(%string, %len)    
{
    if(%len >= String::len(%string))
        return %string;
    
    %left = String::getSubStr(%string, 0, %len);
    return %left;
}

function String::right(%string, %len)    
{
    if(%len >= String::len(%string))
        return %string;

    %idx = String::len(%string) - %len;
    %right = String::getSubStr(%string, %idx, %len);
    return %right;
}

function String::starts(%string, %search)    
{
    %idx = String::len(%search);
    if(%idx > String::len(%string))
        return false;
    
    if(String::left(%string, %idx) == %search)
        return true;
    else
        return false;
}

function String::ends(%string, %search)    
{
    %idx = String::len(%search);
    if(%idx > String::len(%string))
        return false;
    
    if(String::right(%string, %idx) == %search)
        return true;
    else
        return false;
}

function String::insert(%string, %insert, %idx)    
{
    %front = String::left(%string, %idx);
    %back = String::right(%string, String::len(%string) - %idx);
    %result = %front @ %insert @ %back;
    return %result;
}

function String::replace(%string, %search, %replace)    
{
    if (%search == %replace || %search == "")
        return %string;
        
    %result = "";
    while((%idx = String::findSubStr(%string, %search)) != -1)             
    {   
        %len = String::len(%string);
        %front = String::getSubStr(%string, 0, %idx);
        %idx += String::len(%search);
        %back = String::getSubStr(%string, %idx, %len - %idx);
        %result = %result @ %front @ %replace;
        %string = %back;
    }
    return %result @ %string; 
}

function String::doubleSlashes(%string)    
{
    %newstring = "";
    for(%i = 0; %i < String::len(%string); %i++)
    {
        %char = String::getSubStr(%string, %i, 1);
        if(%char == "\\")
            %newstring = %newstring@"\\\\";
        else
            %newstring = %newstring@%char;
    }
    return %newstring; 
}

function String::halveSlashes(%string)    
{
    %newstring = "";
    for(%i = 0; %i < String::len(%string); %i++)
    {
        %chars = String::getSubStr(%string, %i, 2);
        if(%chars == "\\\\")
        {
            %newstring = %newstring@"\\";
            %i++;
        }
        else
            %newstring = %newstring@String::getSubStr(%chars, 0, 1);
    }
    return %newstring; 
}

function String::setString(%chars, %num)
{
    for(%i = 0; %i < %num; %i++)
        %string = %string @ %chars;
        
    return %string;
}

function String::pad(%string, %char, %width, %justify)
{
    %len = String::len(%string);
    if(%len < %width && String::len(%char) == 1)
    {    
        %justify = String::getSubStr(%justify, 0, 1);
        if(%justify == "l" || %justify == "L")
        {
            %string = %string @ String::setString(%char, %width - %len);
            return %string;
        }
        if(%justify == "r" || %justify == "R")
        {
            %string = String::setString(%char, %width - %len) @ %string;
            return %string;
        }
        if(%justify == "c" || %justify == "C")
        {
            %right = (%width - %len) / 2;
            %string = %string @ String::setString(%char, %right);
            %left = %width - String::len(%string);
            %string = String::setString(%char, %left) @ %string;
            return %string;
        }
    }
    return %string;
}

function String::trim(%string)
{
    while(String::starts(%string, " "))
    {
        %string = String::right(%string, String::len(%string) - 1);
    }
    while(String::ends(%string, " "))
    {
        %string = String::left(%string, String::len(%string) - 1);
    }
    return %string;
}

function String::indexOf(%string, %search, %idx)
{
    %newString = String::getSubStr(%string, %idx, String::len(%string) - %idx);
    %newIdx = %idx + String::findSubStr(%newString, %search);
    return %newIdx;
}

function String::len(%string) 
{
    for(%length=0; String::getSubStr(%string, %length, 1) != ""; %length++)
    {} // it's all done above!
    return %length;
}
           
function String::getWordCount(%string)
{
    for(%num = 0; getWord(%string, %num) != -1; %num++)
    {} // it's all done above!
    return %num;
}

function String::ascii(%string, %idx)
{
	if(String::len(%string) <= %idx || %idx < 0)
	    return -1;
	%char = String::getSubStr(%string, %idx, 1);
	%idx = String::findSubStr($String::asciiString, %char);
	if(%idx < 0)
		return -1;

	if(String::Compare(%char, String::getSubStr($String::asciiString, %idx, 1)) == 0)
		return %idx + 32;
    else
        return %idx + 64;
}

function String::char(%ascii)
{
	if(%ascii < 32 || %ascii > 126)
		return "";
	else
		return String::getSubStr($String::asciiString, %ascii-32, 1);
}

function String::pixels(%string)
{
    %pixels = 0;
    for(%i = 0; %i < String::len(%string); %i++)
    {
        %val = String::ascii(%string, %i);
        if(%val != -1)
            %pixels += $String::asciipixels[%val];
    }
    return %pixels;
}
    
$String::asciiString = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

$String::asciipixels[32] = 6;	//	space
$String::asciipixels[33] = 3;	//	!
$String::asciipixels[34] = 4;	//	"
$String::asciipixels[35] = 10;	//	#
$String::asciipixels[36] = 8;	//	$
$String::asciipixels[37] = 13;	//	%
$String::asciipixels[38] = 10;	//	&
$String::asciipixels[39] = 2;	//	'
$String::asciipixels[40] = 5;	//	(
$String::asciipixels[41] = 5;	//	)
$String::asciipixels[42] = 6;	//	*
$String::asciipixels[43] = 8;	//	+
$String::asciipixels[44] = 4;	//	,
$String::asciipixels[45] = 4;	//	-
$String::asciipixels[46] = 3;	//	.
$String::asciipixels[47] = 5;	//	/
$String::asciipixels[48] = 7;	//	0
$String::asciipixels[49] = 3;	//	1
$String::asciipixels[50] = 7;	//	2
$String::asciipixels[51] = 7;	//	3
$String::asciipixels[52] = 7;	//	4
$String::asciipixels[53] = 7;	//	5
$String::asciipixels[54] = 7;	//	6
$String::asciipixels[55] = 7;	//	7
$String::asciipixels[56] = 7;	//	8
$String::asciipixels[57] = 7;	//	9
$String::asciipixels[58] = 3;	//	:
$String::asciipixels[59] = 4;	//  ;
$String::asciipixels[60] = 8;	//	<
$String::asciipixels[61] = 9;	//	=
$String::asciipixels[62] = 8;	//	>
$String::asciipixels[63] = 5;	//	?
$String::asciipixels[64] = 13;	//	@
$String::asciipixels[65] = 11;	//	A
$String::asciipixels[66] = 8;	//	B
$String::asciipixels[67] = 9;	//	C
$String::asciipixels[68] = 10;	//	D
$String::asciipixels[69] = 7;	//	E
$String::asciipixels[70] = 7;	//	F
$String::asciipixels[71] = 10;	//	G
$String::asciipixels[72] = 9;	//	H
$String::asciipixels[73] = 3;	//	I
$String::asciipixels[74] = 5;	//	J
$String::asciipixels[75] = 10;	//	K
$String::asciipixels[76] = 7;	//	L
$String::asciipixels[77] = 11;	//	M
$String::asciipixels[78] = 10;	//	N
$String::asciipixels[79] = 11;	//	O
$String::asciipixels[80] = 8;	//	P
$String::asciipixels[81] = 11;	//	Q
$String::asciipixels[82] = 9;	//	R
$String::asciipixels[83] = 8;	//	S
$String::asciipixels[84] = 9;	//	T
$String::asciipixels[85] = 9;	//	U
$String::asciipixels[86] = 10;	//	V
$String::asciipixels[87] = 14;	//	W
$String::asciipixels[88] = 11;	//	X
$String::asciipixels[89] = 11;	//	Y
$String::asciipixels[90] = 10;	//	Z
$String::asciipixels[91] = 5;	//	[
$String::asciipixels[92] = 5;	//	\
$String::asciipixels[93] = 5;	//	]
$String::asciipixels[94] = 9;	//	^
$String::asciipixels[95] = 8;	//	_
$String::asciipixels[96] = 4;	//	`
$String::asciipixels[97] = 7;	//	a
$String::asciipixels[98] = 7;	//	b
$String::asciipixels[99] = 6;	//	c
$String::asciipixels[100] = 7;	//	d
$String::asciipixels[101] = 7;	//	e
$String::asciipixels[102] = 6;	//	f
$String::asciipixels[103] = 8;	//	g
$String::asciipixels[104] = 7;	//	h
$String::asciipixels[105] = 3;	//	i
$String::asciipixels[106] = 4;	//	j
$String::asciipixels[107] = 8;	//	k
$String::asciipixels[108] = 3;	//	l
$String::asciipixels[109] = 11;	//	m
$String::asciipixels[110] = 7;	//	n
$String::asciipixels[111] = 8;	//	o
$String::asciipixels[112] = 7;	//	p
$String::asciipixels[113] = 7;	//	q
$String::asciipixels[114] = 6;	//	r
$String::asciipixels[115] = 6;	//	s
$String::asciipixels[116] = 6;	//	t
$String::asciipixels[117] = 7;	//	u
$String::asciipixels[118] = 8;	//	v
$String::asciipixels[119] = 11;	//	w
$String::asciipixels[120] = 8;	//	x
$String::asciipixels[121] = 8;	//	y
$String::asciipixels[122] = 7;	//	z
$String::asciipixels[123] = 6;	//	{
$String::asciipixels[124] = 2;	//	|
$String::asciipixels[125] = 6;	//	}
$String::asciipixels[126] = 9;	//	~
