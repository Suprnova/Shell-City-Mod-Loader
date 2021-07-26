
# Shell City

<p align="center">
  <img width="500px" src="_assets/shell%20city%20demo.png">
</p>

Shell City is a mod installer designed for The SpongeBob SquarePants Movie and Battle for Bikini Bottom on GameCube platforms, specifically optimized for Dolphin. If you are interested in distributing your mod in a format that is supported by Shell City, see below.

## Info For Mod Creators

The process of making your mod compatible with Shell City is to create a folder that contains the following:
```
Mod
 - files
   - b1
   - b2
   - b3
   - etc.
 - {game}.ini
 - cover.jpg
```

The .ini should appear to be something like this.

```ini
[Installer]
Game={Base Game Name}
Name={Mod Name}
Authors={Author}
Description={Description}
Directory=files
Image=cover.jpg
```

An image is not required, and as such the "Image" field can be left empty. All other fields are required, and your ini will not load properly should it be formatted incorrectly.

For example, here is how your files should look in File Explorer.

![File Explorer](https://media.discordapp.net/attachments/603730493074046978/867168173807239178/unknown.png)

And here's how your ini file should look:

```ini
[Installer]
Game=Battle for Bikini Bottom
Name=BFBBMix
Authors=skyweiss
Description=Fan-made remix of Battle for Bikini Bottom.
Directory=files
Image=cover.jpg
```

## License and Disclaimers

This program is licensed under the Do What The F*ck You Want To Public License (WTFPL). More information can be found at [LICENSE](https://github.com/Suprnova/Shell-City-Mod-Loader/blob/main/LICENSE). 

SpongeBob SquarePants: Battle for Bikini Bottom (BFBB) and The SpongeBob SquarePants Movie (TSSM) are owned by their respective copyright holders, including but not limited to Heavy Iron Studios, Nickelodeon, and Viacom.

This program uses Poppins under the Open Font License—Copyright 2020 The Poppins Project Authors (https://github.com/itfoundry/Poppins)

Icons made by <a href="https://www.freepik.com" title="Freepik">Freepik</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a>

Icons made by <a href="https://www.flaticon.com/authors/kiranshastry" title="Kiranshastry">Kiranshastry</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a>
