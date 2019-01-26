# MathJax

  <br><br><br>




<math xmlns="http://www.w3.org/1998/Math/MathML" mathbackground="lightskyblue">
<mfrac>
  <mo> &#x2146;<!--DOUBLE-STRUCK ITALIC SMALL D--> </mo>
  <mrow>
    <mo> &#x2146;<!--DOUBLE-STRUCK ITALIC SMALL D--> </mo>
    <mi> x </mi>
  </mrow>
</mfrac>
</math>



<br><br>

<Math>        
<mi >a</mi><mo > + </mo><mn mathsize="4.0em">2</mn>
</math>


<br><br><br><br><br>

$ \(     8 + 202x + 18x^2 + 7x^3 + x^4           \) $

$\rm\TeX $
$\rm\LaTeX $

<br><br><br><br><br><br><br><br><br><br>
## The following links may be useful: 

- [Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet)
- [Ideas for Anyone Who Wants to Understand Mathematics ](http://www.onemathematicalcat.org/cat_book.htm)
- [LaTeX/Mathematics](https://en.wikibooks.org/wiki/LaTeX/Mathematics)
- Need to define your own macros?
[MathJax](https://www.mathjax.org/) supports both   \def   and   \newcommand .
You must include your definitions within a math block, e.g., inside   `$ $` or   `\( \)`   or   `$$  $$`, so that MathJax will process them. 
- [Syntax for TeX Commands](http://www.onemathematicalcat.org/MathJaxDocumentation/notationUnits.htm) available in MathJax gives information about the syntax used in this documentation to describe commands.
It also includes a table of length units available in MathJax. 
- The [MathJax Users Group](http://groups.google.com/group/mathjax-users/) is a support forum and open discussion for the MathJax Project.
Please be sure to read the MathJax documentation and search the forum discussions before creating a new post,
to see if your question has already been answered. 

## Tips
To use inline LaTex, eclose LaTex code with `$`:
$ \(k_{n+1} = n^2 + k_n^{2 + n} - k_{n-1}.\) $ Alternatively,
you can also use `$$`.

$$
 $A_{m,n} = 
 \begin{pmatrix}
  a_{1,1} & a_{1,2} & \cdots & a_{1,n} \\
  a_{2,1} & a_{2,2} & \cdots & a_{2,n} \\
  \vdots  & \vdots  & \ddots & \vdots  \\
  a_{m,1} & a_{m,2} & \cdots & a_{m,n}
 \end{pmatrix} $
$$


Use LaTex escape rule:

- Escape \$ in inline mode: $ \( \$ \) $, $\( \$var \)$ 
- Other escapes: $ \(\&\ \%, \$ \ \# \ \_ \ \{ \ \} \ \) $
- Using < or >: $ \( x > 1, \ y < 1, \ x >= 1, \ y <= 1, \ x = 1\) $
- $ \(<p>something</p>\) $

## Syntax for $\rm\TeX $ Commands available in MathJax
The following syntax is used in TeX Commands available in MathJax: 

**ARGUMENTS:**
Arguments are denoted by #1, #2, #3, etc. Multi-token arguments should be enclosed in (curly) braces: ‘ { } ’ 

**GROUPING CONSTRUCTS:**
There are two basic grouping constructs that use braces; I refer to them as ‘arguments’ versus ‘braced groups’. If you're not aware which construct is in force, then you can get unexpected results.

**DIMENSIONS:**
⟨dimen⟩ denotes: ⟨optional sign⟩⟨number⟩⟨unit⟩ Examples:   -5pt   or   -5 pt   or   3.5pt

**CLASS INFORMATION:**
Math operators are divided into several distinct classes, which control the spacing between elements in the typeset expression.For example, REL uses a little more space than BIN. 
- **ORD:**   an ‘ordinary’ item, like a variable name or Greek letter 
- **OP:**   a ‘big operator’, usually having moveable limits (though not always)and different sizes for display and in-line modes (though not always)
- **BIN:**  a ‘binary operator’ like + and +
- **REL:**   a ‘binary relation’ like < and ≤
- **OPEN:**   an ‘opening delimiter’ like (
- **CLOSE:**   a ‘closing delimiter’ like )
- **PUNCT:**   a ‘punctuation’ like :
- **INNER:**   a special class used for fractions and some other things 
- **DELIMITERS:** Delimiters are symbols used to enclose expressions (e.g., parentheses, brackets, and braces) or used as operators (e.g., vertical lines for absolute value). In MathJax, delimiters can be of class OPEN, CLOSE, REL, or ORD. 
- **BROWSER-SPECIFIC SUGGESTIONS:** Set explicit widths for table-cells that contain math content; in native MathML environments, some unusual line-breaking in math can occur otherwise.

**DIMENSION UNITS:**

| Unit: | Description: | Size: |
| :-: |:-|:-|
|em|a relative measure; depends on current font|approximately the width of capital ‘M’ in current font| 
|ex|a relative measure; depends on current font|$\(1\text{ ex} = 0.43\text{ em}\)$<br>approximately the height of lowercase ‘x’ in current font; gives information about the height of lowercase letters| 
|pt<br>point|a relative measure; depends on current font; not affected by superscript level|$\(1\text{ pt} = \frac{1}{10}\text{em}\)$|
|pc<br>picca|a relative measure; depends on current font; not affected by superscript level|$\(1\text{ pc} = 12\text{ pt}\)$|
|mu|a relative measure; depends on current font; changes with superscript level |$\(1\text{ mu} = \frac{1}{18}\text{em}\)$|
|cm<br>centimeter<br>mm<br>milimeter|absolute measure; does not depend on current font|$\(10\text{ mm} = 1\text{ cm}\)$|
|in||$\(1\text{ in} = 2.54\text{ cm}\)$|
|px|screen pixel||

## $\rm\TeX $ Commands available in MathJax 
| Symbol: | Use: | Example: |
| :-: |:-| :-|
| \# | indicates numbered arguments in definitions |  \( \def\specialFrac#1#2{\frac{x + #1}{y + #2}} \specialFrac{7}{z+3}\)<br><br>yields:<br><br> $ \( \def\specialFrac#1#2{\frac{x + #1}{y + #2}} \specialFrac{7}{z+3}\) $ |
|%|used for a single-line comment; shows only in the source code; does not show in the rendered expression||

