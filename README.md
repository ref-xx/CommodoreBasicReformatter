﻿# Reformat and augment BASIC for improved readability - GUI Version


This tool makes Commodore BASIC programs more readable! It supports a variety of strategies for achieving this. It adds whitespace, it breaks multiple statements into several lines and finally, it
explains character codes and memory locations used in the code. The later is of particular importance. Unlike modern programming, accesing specific memory addresses in Commodore BASIC is used 
for playing sounds, controlling the keyboard buffer size or perhaps most famously, changing the background and border colors. 

![c64 basic](c64.png)




## 1. Usage

    CommodoreBasicReformatter [--split-lines] [--add-explanations] <infile> <outfile>

Gui version uses buttons and checkmarks.

![gui](cbrguiwin.jpg)


## 2. Example

An example code from the fun game "Raging robots" found at http://www.gb64.com/game.php?id=21265

```
250 vl=54296:ad=54277:p1=54273:p3=54287:wf=54276
260 qa=198:qb=214:sc=1024:cs=55296:pokeqa,0:z$=""""
270 rt=ti:wait qa,1:getz$:pokeqa,0:rq=ti-rt
280 rr=rq*rnd(0):dim r(nr),p(nr):r=nr
290 print""."":fori=1to80:pokevl,5:pokevl,0
```

The code is so dense. If we reformat it with our tool, it becomes

```
250 vl = 54296 : ad = 54277 : p1 = 54273 : p3 = 54287 : wf = 54276
260 qa = 198 : qb = 214 : sc = 1024 : cs = 55296 : poke qa,0 : z$ = """"
270 rt = ti : wait qa,1 : get z$ : poke qa,0 : rq = ti-rt
280 rr = rq*rnd(0) : dim r(nr),p(nr) : r = nr
290 print ""."" : for i = 1 to 80 : poke vl,5 : poke vl,0
```

which is much nicer to the eye.


### 2.1. The `--split-lines` argument 
 But we can improve readability even more by using the `--split-lines` flag. Then the code becomes

```
250 vl = 54296
251 ad = 54277
252 p1 = 54273
253 p3 = 54287
254 wf = 54276
260 qa = 198
261 qb = 214
262 sc = 1024
263 cs = 55296
264 poke qa,0
265 z$ = """"
270 rt = ti
271 wait qa,1
272 get z$
273 poke qa,0
274 rq = ti-rt
280 rr = rq*rnd(0)
281 dim r(nr),p(nr)
282 r = nr
290 print "".""
291 for i = 1 to 80
292 poke vl,5
293 poke vl,0
```

Not all lines are split, however. Then-blocks are excluded as it would change the semantics if the code. For example, the line:

```
390 da=asc(d$) :ifda=145thendp=-40:goto480
```
becomes

```
390 da = asc(d$)
391 if da = 145 then dp =-40 : goto 480
```

### 2.2. The `--add-explanations` argument

The tool makes semantic analysis of the code and suggest explanations as comments. The analysis covers

  * Known memory location usage
  * Known `chr$` codes.

We explain variables and constants that we can conclude are used as memory locations through a simple analysis.
Let's try adding explainations to this very nice 6-line smooth screen scroller written in basic.

```
10 print chr$(147) : poke53280,6:poke53281,0
20 for l=1024 to 2023:poke l,219:next l
30 for l=0 to 7
40 poke 53265, (peek(53265) and 240) or 7-l : poke 53270,l
50 next l
60 goto 30
```

You can see it in execution on https://youtu.be/JzPEfHufyfg?t=93 The original code is a bit cryptic and the author of the video goes into some details as to explaining each line:
But our tool can do some explaning of its own! 

Take a look at the code when we enhance it with explanations:

```
10 rem 53280=Border color
10 rem 53281=Background color
10 rem 147=Clears screen of any text, and causes the next character to be printed at the upper left-hand corner of the text screen.
10 print chr$(147) : poke 53280,6 : poke 53281,0
20 rem 1024=Default first area of screen memory (upper left corner)
20 rem 2023=Default last area of screen memory(lower right corner)
20 for l = 1024 to 2023 : poke l,219 : next l
30 for l = 0 to 7
40 rem 53265=Screen control register #1 (e.g. for YSCROLL or setting gfx mode)
40 rem 53270=Screen control register #2 (e.g. for XSCROLL or setting gfx mode))
40 poke 53265,(peek(53265)and 240)or 7-l : poke 53270,l
50 next l
60 goto 30
```

The line duplication is intentional. It allows you to keep the explanations you like simply by changing their line numbers. Otherwise
they get overwritten by the actual code that produced the explanation.


## 3. How it works

We do light-weigh tokenization of the following sets of characters


We've made a simple parser using the following grammar
    

     KEYWORD ::= 'to' | 'then' | 'end' | 'for' | 'next' | 'data' | 'dim' | 'read' | 'let' | 'goto' | ...
     SYMBOL  ::= ',' | '+' | '-' | '*' | '/' | '(' | ')' | '=' | '<' | '>' | '<>' | ';' | '#'
     STRING  ::= 'a'..'z' ( 'a'..'z' | '0'..'9' | '%' | '$' )*
     DIGIT   ::= ( '0'..'9' )+
     NEWLINE ::= '\n'
     COLON   ::= ':'


We then parse a file as a stream of tokens. The tokens has to obey the following simple grammar

     PROGRAM ::= ( NEWLINE | LINE )*
     LINE    ::= DIGIT STMTS NEWLINE
     STMTS   ::= STMT ( COLON STMT )* 
     STMT    ::= ( KEYWORD | DIGIT | STRING | SYMBOL )+

On top of this we apply the various analysis and content modification before outputting the result.




## 4. Working with tape and disc files

C64 BASIC listings are easy to get hold of from the internet. Usually they come in the form of tape or disc images - thus not being readily readable. 
The VICE emulator (http://vice-emu.sourceforge.net/) comes with two essential tools `c1541.exe` and `petcat.exe` which are needed in order to read the BASIC code listings on
more modern operating systems.


### 4.1. How to extract and reformat T64 files 

Before reformatting tape files, you need to convert the tape file to a disc image, then extract to the local file system, and then run `petcat` on it.

An example with a T64 file named `RAGINGRO.T64` containing the BASIC file `raging robots`

    .\c1541.exe -format diskname,id d64 my_diskimage.d64 -attach my_diskimage.d64
    .\c1541.exe -attach my_diskimage.d64 -tape RAGINGRO.T64
    .\c1541.exe -attach my_diskimage.d64 -extract
    .\petcat.exe -o rr.ascii "raging robots"
    dotnet CommodoreBasicReformatter.dll ".\rr.ascii" rr2.ascii

to make an executable of the re-formatted program run

    .\petcat.exe -w2 -l 0x0801 -o rr2.prg rr2.ascii

then watch it run in VICE 

	x64.exe rr2.prg


### 4.2. How to extract and reformat D64 files 

Before reformatting disc files, you need to extract to the local file system, and then run `petcat` on it.

An example with a D64 file named `YADA.D64` containing the BASIC file `DD.PRG`

    .\c1541.exe -attach YADA.D64 -extract
    .\petcat.exe -o dd.ascii "dd"
    dotnet CommodoreBasicReformatter.dll ".\dd.ascii" dd2.ascii

to make an executable of the re-formatted program run

    .\petcat.exe -w2 -l 0x0801 -o dd.prg dd2.ascii

then watch it run in VICE 

	x64.exe dd2.prg


## 5. Supported platforms

The code is written in .Net core so it should run on Windows, Linux, MAC OS

Download a binary release from https://github.com/kbilsted/CommodoreBasicReformatter/releases/ or build the source code.

If you cannot find a runtime for your Linux distribution, the tool works fine through Wine.


## 6. Have fun

Kasper Graversern

http://firstclassthoughts.co.uk/
