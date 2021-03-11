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

