# fesch

fesch = Final Exam Scheduling

/// terminológia
/*
 * vizsgaszerkezet = 
 *     examStructure = 
 *     a vizsgaidőszak felépítése: tartalmazza az elnökök, titkárok, napok, termek összességét
 * 
 * vizsga résztvevői =
 *     examAttendants =
 *     a vizsgán résztvevő személyek összessége: hallgató, vizsgáztató, tag, konzulens, 
 *     ergo mindenki az elnökök és titkárok kivételével
 */

dependenciák
EPPlus nuget package, verzió: 5.7.5
Gurobi (https://www.gurobi.com/downloads/gurobi-optimizer-eula/), verzió: 9.0.2
 - itt project explorer, References jobb klikk, és Gurobi90.NET.dll-t kell behúzni
 - tipikus badImageFormatException
  - állítsuk a debug platformot explicit x86 vagy x64-re, én x64-en teszteltem
  - nincs megújítva az academic license, ehhez (https://www.gurobi.com/downloads/licenses/)
.
