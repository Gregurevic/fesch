# Final Exam Scheduling (fesch)

## Mire van szükség, hogy elindítsam a projektet?
  1. szükséges szoftverek: Visual Studio (2019+), Gurobi optimizer (9.0.2+), Git
  2. git clone project, nyisd is meg VS-ben, majd
  3. add hozzá ezt a nuget package-et: EPPlus (5.7.5)
  4. utána jobb klikk a "References"-en, és add hozzá a Gurobi90.NET.dll-t
  5. ha ezen a ponton még BadImageFormatException-t kapsz, akkor ellenőrizd le a következőket:
    - állítsd be a dubog platformot explicit x86 vagy x64-re (én x64-et használtam)
	- ellenőrizd le, hogy nem járt-e le a Gurobi Academic License-ed (https://www.gurobi.com/downloads/licenses/)

## Terminológia
Ahhoz, hogy otthonosan tudjak mozogni a záróvizsgabeosztás problémakörében, és hogy ne legyen magyar szavakkal teletűzdelve a kód, be kellett vezetnem pár kifejezést. Ezeknek a magyarázatait alább olvashatod.

| Kódban használt kifejezés | Jelentés |
| ------------------------- | ----------- |
| Structure                 | A vizsgaidőszak felépítése: tartalmazza az elnökök, titkárok, napok, termek összességét. Sokszor az első beosztási folyamatot jelöli. |
| Attendant                 | A vizsgán résztvevő személyek összessége: hallgató, vizsgáztató, tag, konzulens. Sokszor a második beosztási folyamatot jelöli. |
| Fragment                  | Egy darab nap-terem-elnök-titkár összerendelést reprezentáló objektum. |
| I                         | dimenzió - összes oktató száma |
| D                         | dimenzió - a beosztott időszak hossza napokban mérve |
| C                         | dimenzió - rendelkezésre álló, vagy kihasznált, termek száma |
| S                         | dimenzió - osszes hallgató száma |
| F                         | dimenzió - összes Fragment száma (második beosztásban van csak értelmezve) |
| O                         | dimenzió - sorszám avagy maximálisan egy napra beosztható hallgatók száma |
| OS                        | dimenzió - rövid(vizsga) sorszám avagy maximálisan egy napra beosztható magyar BSc-s hallgatók száma |
| OL                        | dimenzió - hosszú(vizsga) sorszám avagy maximálisan egy napra beosztható angolos vagy mesterképzésen lévő hallgatók száma |
