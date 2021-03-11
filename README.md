[Digitális tankönyvtár: A gépi látás és képfeldolgozás párhuzamos modelljei és algoritmusai](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/ch02.html)

# Számítógépes képfeldolgozás

- képfeldolgozás: https://users.nik.uni-obuda.hu/vamossy/Kepfeldolgozas2019/
- gépi látás: https://users.nik.uni-obuda.hu/vamossy/GepiLatas2019/

**követlemény:**
- 1 beadandó bejelenető március elejéig
- 2 beadandó 
  - kis photoshop: *fontos az algoritmusok gyorsítása!*
    - Negálás *
    - Gamma transzformáció
    - Logaritmikus transzformáció
    - Szürkítés *
    - Hisztogram készítés
    - Hisztogram kiegyenlítés
    - Átlagoló szűrő (Box szűrő)
    - Gauss szűrő
    - Sobel éldetektor
    - Laplace éldetektor
    - Jellemzőpontok detektálása
  - saját téma, pl: kép elemzés, biometrikus adat elemzés, műholdkép elemzés, MR, stb
- 1 zh a félév végén

# EA 1
- [Richard Szeliski könyve!](http://szeliski.org/Book/)

> **technika fejődése:**
> - orvoslás, ICT fejlődése
> - kémműholdak 
> - katonai fejlesztések

## mi a digitálsi tér fogalma:
- kétdimenziós függvény ahol az x és y koodináták f amplitódújú az (x,y) koordinátákban a intenzitás vagy a szürkeségi szint
- ha x,y,f diszkrét mennyiségek akor a képet digitálsinak mondjuk

![mintavételezés és kvantálás 1 és 2D-ben](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/02_04.png)![digitális kép és intenzitás reprezentációja](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/02_06.png)

## Mi a képfeldolgozás?
- Szegmentálás (részekre bontás), leírók kinyerése
- Osztályozás, analízálás, megértés
> **Digitális képfeldolgozás (Digital image processing, DIP):** digitális képek feldolgozása digitális számítógépekkel; képek fokozása, vagy más manipulálása, az eredmény általában másik kép (és valamilyen jellemzők)
> 
> **Számítógépes látás, vagy röviden gépi látás (Computer Vision, CV):** számítógép használata az emberi látás emulációjára, amely magába foglalja a tanulást, a következtetést és a reagálást (leírás, analízis, megértés). A mesterséges intelligencia (Artificial Intelligence, AI) több részét használják a CV-ben, mint a DIP-ben

### Zaj kezelése
#### ismétlődő zaj
> ![ismétlődő zajos kép](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/02_10.png)
> 
> mivel a képfeldolgozás sokszor az ilyen átsázmítást használja fel különböző jellemzők elemzésére
> - durva válltozás
> - hirtelen változás a kontúrok mentén
> - adott közte tartományt próbál hatékonyan kezelni
> 
> ![tömörítés](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/02_11_2.png)
> 
> javasolt nagyobb terjedelmű nyers formákat használni, szemben a nagyobb tömörítést használó képekkel, mert a tömörített kép visszafejtése szinte sosem teljes, valmaint tömörítés után már kis anomáliák is megjelenhetnek

#### A digitális képfeldolgozás szintjei
- Alacsony szint: mind az input mind az output kép
- Közép szint: képek részekre bontása valamiylen jellemző alapján, pl hasonló sínek, fomrák, stb
- Magas szint: a felismert objektumok együttesének érzékelése, mesterséges inteligencia használatával, osztályokat határozunk meg, analizálunk jeleneteket
![kép feldolgozás](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/02_14.png)

részekre bontás:

![kép részekre bontása](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/02_15.png)![MI felbontás](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/02_16.png)

## Mi a gépi látás (Computer Vision)?
> térbeli modellek visszaállíása képekből, képsorozatokból

- milyen kamerákat használunk, vagy akmerák vagy szenzorok
- pl motion capture: felvesszük a mozását egy objektumnak, és rávetítjük egy 3d avatarra.

![motion capture](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/02_19.png)

### Miért bonyolult a számítógépes látás?
Jelenetek megértése, még komplex és rendezetlen kép esetében is egyszerű az ember számára, a gép számára viszont a külön egységek azok amiket lát, ért, a tlejes egész az amit nem. 

> **szín szerepe**:  A színek intenzitását figylehetjük, de fontos, hogy milyen színmodellt használunk ekkor, mert a használt fényintenzitás változtathat a színeken, és csalóka lehet.
> 
> **textúra**: a textúrák használata is jó lehet de bizonyos ismétlődést nem tud kizárni.
> 
> **alak szerepe**: a formák észlelése is érdekes lehet, de kérdés, hogy például az árnyék és hasonló dolgok emnnyire befolyásolják az alak értlemezését
> 
 



# EA 2
### hisztogram kiegynelítés
- minden egyes képnek elkészítjük a hisztogramját
- kiszámoljuk minden pixelre a relatív értéket, majd 

### átlagolás
> képminőség javítása több kép átalgával
>
> sok zajos kép esetén a zaj átlaga nulla
 
### kivonás
> változást lehet detektálni 
> - készítünk egy háttérmodellt *pl utcakép*
> - megjön a változás, kivontjuk a kettőt egymásból, megkapjuk a változást magát *pl jármű*

### and/or képre alkalmazva
> a számunkra érdekes részt lehet kiemelni, ha az alp képre egy fehér alapon fekete vagy fekete alapon fehér kijelölést jelző képel összeéseljük vagy vagyokljuk

### képek transzformálása/inverz transzformálása
> forgatás/eltolás esetén az eredeti képből az elforgatás után készült képen lehetnek lyukak amikhez nem tartozik eredeti képpont ekkor inverz transzformációval visszaszámolhatjuk a szomszédos képpontokat kiemelve behelyettesíhetjük.

## Invert Filter
forrás: https://www.codeproject.com/Articles/1989/Image-Processing-for-Dummies-with-C-and-GDI-Part-1
```C#
public static bool Invert(Bitmap b)
{
    // GDI+ still lies to us - the return format is BGR, NOT RGB. 
    BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), 
        ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);  //a BitMap nem rgb hanem bgr sorrendben tároljaa színeket!
    int stride = bmData.Stride;  //képsor futam, azt adja a meg, hogy a memóriában a kép milyen széles legyen, ugynaakkor ez nem feltétlen egyenlő a teljes képszélességgel, csak ha az pontosan osztható vele
    System.IntPtr Scan0 = bmData.Scan0; //egy olyan pointer ami rámutat a memóriterület kezdetére ahol a bitmapp kezdődik
    unsafe 
    { 
        byte * p = (byte *)(void *)Scan0;
        int nOffset = stride - b.Width*3; 
        int nWidth = b.Width * 3;     //
        for(int y=0;y < b.Height;++y) //soronként fledolgozzuk a képet
        {
            for(int x=0; x < nWidth; ++x )  //byteonként feldolgozzuk a sort //lehetne e helyett a teljes sort negálni is
            {
                p[0] = (byte)(255-p[0]);  //negálja a pixelt
                ++p;
            }
            p += nOffset; //sort ugrunk
        }
    }

    b.UnlockBits(bmData);

    return true;
}
```

## Grayscale filter
```C#
unsafe
{
    byte * p = (byte *)(void *)Scan0;

    int nOffset = stride - b.Width*3;

    byte red, green, blue;

    for(int y=0;y < b.Height;++y)
    {
        for(int x=0; x < b.Width; ++x )
        {
            blue = p[0];
            green = p[1];
            red = p[2];

            p[0] = p[1] = p[2] = (byte)(.299 * red 
                + .587 * green 
                + .114 * blue); //gyorsabb volna ha egésszé alakítanánk vagy lookup tablet használnánk

            p += 3;
        }
        p += nOffset;
    }
}
```

## Brightness filter
```C#
for(int y=0;y<b.Height;++y)
{
    for (int x = 0;  x < nWidth; ++x)
    {
        nVal = (int) (p[0] + nBrightness);

        if (nVal < 0) nVal = 0;
        if (nVal > 255) nVal = 255;

        p[0] = (byte)nVal;

        ++p;
    }
    p += nOffset;
}
```

### szűrés képtéren
**Maszk használata**
- adott input minden pixelére maszkot helyezünk, úgy, hogy annak origója az adott pixelekre essék
- az input maszk alatti pixleeit megszorozzuk a maszkban szereplő súlyokkkal
- az eredmény: az input helyzetének megfelelő pixel 

![maszk súlyok és pixelek közi összefüggés](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/05_15.png)
#### lineáris szűrés
> megszorzunk súly intenzitásokat és úgy akpunk kép értékeket
> 
> az ouput lineáris
> - simító: átlagoló
> - élesítő
>   - alul/felül áteresztő szűrők
>   - sávszűrők
>   - derivált szűrők

#### nem lineáris szűrés
- medián szűrő
- simítás

# EA 3
## Nem lineáris szűrők, medián szűrő
- impulus szerű zajokat eltávolíŧ
- nem szélesíti ki az éleket
- nem mindig hatékony
 
## élesítés
> élesítéshz térbeli deriváltakat használunk -> gradiens két irányból
> - számoljuk ki x és rá merőleges y irányban a derivált értékét
> - a két értékből sázmítsunk vektor eredőt, és annak az irányát és nagyságát is határozzuk meg.
> 
> ### Laplace szűrő
> `x` irányú derivált és `y` irányú drivált összege
> 
 
## él detektálás
> sarok pont/jellemző pont detektálás

- ha éleket akarunk detektálni előbb használjunk zajszűrést
- élkiemelés
- éldetektálás
- éllokalizálás

euklédeszi távolság: négyzetek összegének gyöke

manhattan távolság: abszolút értékben a két irány nagyságának összege

### Prewitt operátor
- átlagolás egyik irányban
- a maszk elemek összértéke nulla


### Canny éldetektor 
- élkiemelés
- nem maximumok elnyomása
- histerézises küszöbölés
- gauss zajos képeken
- lépcsős képeken használható

ideális éldetektálás:
1. gauss szűréssel simitás
2. kétirányú gradiens számítás
3. normális és erősség számítás
4. kiszámolja az élnormális irányát és  elvékonyítja ott a képet
5. összeköttjük az élpontokat láncolt listákban
6. hysteresises küszöbölés

# EA 4

# EA 5 - vonalak detektálása
> előzetesen feltételezzük, hogy éldetektálást végzünk, és vannak különálló pixeleink amiket meghatároztunk

cél: az E(x,y) képen találjuk meg a vonalakat és határozuk meg azok egyenletét

> ## [Hough transzformáció](https://hu.qaz.wiki/wiki/Hough_transform)
> 
> *kulcs:* használjunk paraméterekt ahol a bonyolult probléma az egyszerűbb lokális maximumok megtalálását jelenti
> 
> *input:* nináris kép képpontokkal
> 
> *Meredekség értékékeben gondolkodunk, megtalálunk egy élpontot, arra meredekséget tételezünk fel, arra vannak `m` értékei és koordinátái, amire behelyettesítünk `x`,`y`-t, ez alapján kapunk `b=-mx+y` jelleggel.*
> 
> ![meredekségek](https://image2.slideserve.com/4449132/hough-transzform-ci-l.jpg)
> 
> 1. Az `(m,b)` teret osszuk fel egy rácsalés mindenhez rendeljünk egy számlálót `c(m,b)` kezdetben 0 értékkel => `bi=y-mjx`
> 2. minden élpixel ismert koordinátáival számoljuk ki a`b` értékét` minden lehetséges `m` mellett
> 3. növeljök =c(mi,bi)`-t egyel
> 4. keressünk lokális maximumokat a képen
> 
> kvantálás:
> 
> ! [kvantálás](https://image2.slideserve.com/4449132/hough-traf-kvant-l-s-l.jpg)
> 
> A művelet viszont 3 egymásba ágyazott ciklus, a számítási jgény kisebb, viszont az `x,y` tengelyen 0-tól végtelenig vehetünk fel értékeket amihez elég angy memória kell.
> 
> Az egyenes egynelet nem csak Descartesi de polár koordinátával is leírható, így a tengely meredekséget vesszük figyelembe, hogy milyen szöget zár be a függőleges tengelyel. Másik kérdés, milyen messz eszerepel az origótól (bal felső saroktól). Ez alapján az egens egyenlete: `xcosA + ysinA = p`
> 
> ![polár koordinás reprezentálás](https://image2.slideserve.com/4449132/hough-transzform-ci1-n.jpg)
> 
> Ez alapján akkor van legközelebb az oigóhoz, ha átmegy rajta, azaz 0 az értéke. A legtávolabbi pont a főátló vége, azaz a kép mérete. A szóbajövő rtékeket 0 és a képátló méretével kell tehát elheyezni sé nem 0 és végtleen között.
> 
> 1. készíünk egy 2D számláló tömböt a szög és a 180 fok változik, a távolság pedig a maximum képátló.
> 2. mindne szög értkére behelyettesítünk valamilyen nvekményt használva (pl 10 fok)
> 3. számoljuk ki minden élpontra a `p` értéket, minden kiszámolt `(A,p)` párra növeljük a sázmlálótömb értékeit.
> 4. keressünk lokális maximumot.
> 
> Ez lokális maximumot és csomósodást fog adni válaszul a paramétere térben. Tehát nagyon zajos kép esetén jelentős hiba állhat fenn.
> 
> ![zajos képpel](https://image2.slideserve.com/4449132/vonaldetekt-l-s-p-lda-n.jpg)
>
> **Nehézségek**
> 
>  - paraméter tér felosztása, nagy felosztással nehéz különbséget teni a vonalak között, kicsivel a zaj okoz gondot
>  - hány vonalat látunk
> - melyi kélpont melyik vonalhoz tatozik?
> - zaj miatt nehéz kielégítő megoldást adni
> 
> ### Kör illesztése
> három ismeretlen: `(x-x0)^2 + (y-y0)^2 - r^2 = 0`
> 
> - 3D akkumulátor tömböt kell készíteni, dimenziók: `x0`, `y0`, `r`
> - lokális maximumot kereesünk A tömbben

gyakorlatban javasolt módszer             |  példák
:-------------------------:|:-------------------------:
![gyakorlatban javasolt módszer](https://image2.slideserve.com/4449132/gyakorlatban-javasolt-m-dszer-n.jpg)  |  ![példák](https://image2.slideserve.com/4449132/p-ld-k-n.jpg)

## Képpiramisok
> https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/ch07.html

- Ha az objektumok képe túl kicsi, vagy nem elég kontrasztos, akkor általában nagyobb felbontással vizsgáljuk azokat
- Ha nagy méretűek, vagy kontrasztosak, akkor elegendő durva felbontás
- Ha mind kicsi, mind nagy, illetve alacsony és nagy kontrasztú objektumaink egyaránt vannak a képen, előnyös lehet különböző felbontással vizsgálni azokat
- A képpiramis olyan hatékony és egyszerű képreprezentáció, aminek segítségével a kép több felbontását használjuk

Más elnevezés: Felbontás hierarchiák (Resolution hierarchies)

> ### Skálázás
> a képek méretének csökkentése a cél, minden lépésben fele méretű legyen, a környező pixeleket valamilyen interpolációval elhagyjuk, majd vissznagyítunk pl bezie splineokat használva.
> 
> 
> kicsinyítés       |  vissza nagyítás
> :-------------------------:|:-------------------------:
> ![kicsinyítés](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/07_06.png)  |  ![nagyítás](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/07_07.png)

Cél: képek tömör reprezentációja, gyors algoritmusok készítése

A képpiramisok (= felbontás hierarchiák) a kép különböző skálázású másolataiból épülnek fel
- A piramis minden szintje az előző szint 1/4-e
- A magasabb szint magasabb felbontást jelent
- A legalacsonyabb szint a legkisebb felbontású
- (Megjegyzés: néha a szintek azonosítása éppen ellentétes e kijelentésekkel)
- A magasabb szint pixelértékeit “redukáló” (Reduce) függvény segítségével számoljuk
`gl= REDUCE[gl+1]`
 
képpiramis működése             |  algorimtus maga
:-------------------------:|:-------------------------:
![képpiramis működése](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/07_14.png)  |  ![algoriitmus](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/07_15.png)

**Piramisok készítése**
1. Minden szinten van egy közelítő képünk és egy különbség (maradék) kép
2. Az eredeti kép (amely a piramis alapja) és az ő P közelítései a közelítő piramist építik fel
3. A maradék outputok a “maradék piramist” építik fel
4. Mind a közelítő, mind a maradék piramisok iterációs módszerrel határozhatóak meg
5. A P+1 szintű piramis a konstrukció algoritmusának P alkalommal történő futtatásakor keletkezik
6. Az első iterációban az eredeti 2J x 2J méretű kép az input
7. Ebből készül a J-1 szintű approximációs és a J szintű maradék eredmény
8. Az iterációk során az előző iteráció eredményét használjuk az új lépés inputjaként

**Minden iteráció három lépésből épül fel:**
1. Számoljuk ki az input kép redukált felbontású közelítését. Ez szűréssel és pixelek leosztásával (downsampling by factor 2) történik
   - Szűrő: szomszédok átlagolása, v. Gauss szűrő, stb.
   - A közelítés pontossága függ a szűrőtől (lásd később)
2. A kapott output pixeleinek felszorzásával (upsampling by factor 2) és szűréssel készül a közelítő kép, aminek a felbontása megegyezik az inputéval.
   - A pixelek közötti interpolációs szűrő meghatározza, hogy mennyire jól közelítjük az inputot az 1. lépésben
3. Számoljuk ki a 2. lépésben kapott közelítés és az 1. lépés inputjának különbségét (maradék). A különbség később az eredeti kép rekonstruálásához használható

![közleítő és maradék piramis](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/07_16.png)

**Alkalmazási területek**
- Hasonló részek keresése
- Keressünk durva skálán, majd finomítsunk nagyobb felbontásnál
- Élkövetés, mozgások vizsgálata
- Minták keresése
- Csíkok keresése
- Nagyon fontos textúrák vizsgálatánál

### Gauss piramis 1D-ben
![gauss1d](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/math/eq_07_03.gif)

- szimetrikus konvolúőciós maszk: `[w(−2),w(−1),w(0),w(1),w(2)]w(i)=w(i)⇒[c,b,a,b,c]`
- A maszk elemeinek összege`a+2b+2c=1`

Minden csomópont egy adott szinten ugyanannyi összsúllyal járul hozzá a következő szinthez:

![szintek súyozása](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/07_19.png)

**Konvolúciós maszkok (5 × 1)**

képpiramis működése             |  algorimtus maga
:-------------------------:|:-------------------------:
![képletek](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/math/eq_07_06.gif)![diagram](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/07_38.png) | a = 0.4 - Gauss maszk a = 0.5 - háromszög maszk a = 3/8 - könnyen számolható maszk ![diagram](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/07_39.png)

### Laplace piramis
- Hasonló az élszűrt képekhez
- A legtöbb pixel 0
- Tömörítésre is használható
- Laplace piramis orientáció független

**Laplace piramis készítése:**

Gauss piramis kiszámítása: `gk,gk-1,gk-2,...g2,g1`

Laplace számítása: Gauss – „visszahízlalt (Expand) előző Gauss”
```
Lk   =gk−EXPAND    (gk−1)
Lk−1 =gk−1−EXPAND  (gk−2)
Lk−2 =gk−2−EXPAND  (gk−3)
⋮
L1   =g1
```

**Képrekonstrukció piramisokból**
- Az eltárolt piramisokból az eredeti kép visszaállítható
- A Laplace piramis jól tömöríthető (a kép homogén részeinél)
````
g1=L1g2 =EXPAND  (g1)+L2
g3      =EXPAND  (g2)+L3
⋮
gk      =EXPAND  (gk−1)+Lk
````

> **Textúra transzfer**
>
> ![texturatranszfer](https://regi.tankonyvtar.hu/hu/tartalom/tamop412A/2011-0063_15_gepi_latas/images/07_44.png)
> 
> - Készítsük el a narancs kép Laplace piramisát (Ln)
> - Készítsük el az alma kép Laplace piramisát (La)
> - Készítsük el a következő összemásolt Lc piramist:
>   - az alma La piramisának bal részét minden szinten és a narancs Ln piramis jobb oldalát minden szinten másoljuk egybe
> - Rekonstruáljuk a kombinált képet Lc-ből
