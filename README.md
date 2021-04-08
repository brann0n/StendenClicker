<h1>StendenClicker</h1>

In opdracht van NHL Stenden is er vanuit de vakken Design Patterns en C# Threading een eindopdracht gemaakt door de projectgroep genaamd 
“Stenden Clicker”. Er is besloten de eindopdrachten van de vakken te combineren welke samen 6EC waard zijn. Het project is gebaseerd op een clicker game. 
Er is een mogelijkheid zijn om met meerdere spelers tegelijk te spelen.

Dit project is opgezet en is ontwikkeld door:

* Brandon Abbenhuis
* Sjihdazi Hellingman
* Mark van der Hart
* Sytze van der Gaag
* Jarno Hilverts
* Jurrian Tanke

<h2>Design Patterns</h2>

De volgende Design Patterns zijn verwerkt:

* <h3>Abstract Factory</h3>

De abstract factory maakt scenes, platforms en monsters aan. Deze zijn te vinden onder: StendenClicker.Library. De code voor de factory ziet er als volgt uit (voorbeeld: [IAbstractScene.cs](https://github.com/brann0n/StendenClicker/blob/master/StendenClicker.Library/AbstractScene/IAbstractScene.cs)):
```C#
public interface IAbstractScene
	{
		public int CurrentMonster { get; set; }
		public int MonsterCount { get; set; }
		public string Background { get; set; }
		public string Name { get; set; }
	}
```
Hier is te zien hoe een scene wordt gemaakt aan de hand van een aantal fields die worden opgehaald en geset. 
Afhankelijk van wat er wordt opgehaald, komt de scene er op een bepaalde manier uit te zien.

* <h3>Object Pool Design</h3>

De StendenClicker maakt gebruik van een object pool design voor het aanmaken en opslaan van coins. Deze kunnen vervolgens worden hergebruikt om recources te besparen.
In StendenClicker.Library/CurrencyObjects staat [ReusableCurrencyPool.cs](https://github.com/brann0n/StendenClicker/blob/master/StendenClicker.Library/CurrencyObjects/ReusableCurrencyPool.cs). Hier is te zien hoe coins worden 
aangemaakt wanneer er niet genoeg van zijn en worden opgeslagen voor hergebruik.

```C#
protected static ReusableCurrencyPool Instance { get { return instance.Value; } }
private readonly List<Currency> Reusables;
private int PoolSizeSC { get { return Reusables.Where(owo => owo is SparkCoin).Count(); } }
private int PoolSizeEC { get { return Reusables.Where(uwu => uwu is EuropeanCredit).Count(); } }
```

Om ervoor te zorgen dat de pool niet oneindig groot wordt is er een limiet op gezet.
```C#
public const int PoolSizeSC_MAX = 50;
public const int PoolSizeEC_MAX = 3;
```

* <h3>Momento</h3>

Het “Player” object wordt omgezet in JSON-data. Deze data worden vervolgens als String opgeslagen in de database door de ApiPlayerHandler. Het ViewModel weet wanneer de data  moet worden opgeslagen. Wanneer er op een fysieke knop wordt geklikt of wanneer de game wordt afgesloten zal de data worden opgeslagen. Via de getPlayerState van de ApiPlayerHandler kan de actie worden teruggedraaid. Dit is waar de design pattern memento van toepassing is. Memento zorgt ervoor dat een object weer terug kan veranderen naar  zijn eerdere staat. Wanneer de speler het spel weer opstart kunnende opgeslagen gegevens gemakkelijk weer worden teruggezet. 

In de onderstaande code wordt de huidige player state opgeslagen na elke monster die verslagen wordt. Deze is te vinden in StendenClicker.Library onder Playercontrols in de file
[ApiPlayerHandler.cs](https://github.com/brann0n/StendenClicker/blob/master/StendenClicker.Library/PlayerControls/ApiPlayerHandler.cs)
```C#
public async Task SetPlayerStateAsync(Player player)
			state = player;
			Models.DatabaseModels.Player dbPlayer = player;
			var response = await RestHelper.PostRequestAsync("api/player/set", dbPlayer);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				await LocalPlayerData.SaveLocalPlayerData(state);
			}
			else
			{
				await LocalPlayerData.SaveLocalPlayerData(state);
				throw new Exception($"Couldn't set the player state... Api error: [{response.StatusCode}] {response.ErrorMessage}");
			}
		}
```

* <h3>Observer</h3>

Binnen de StendenClicker game zal ereen optie zijn om samen met andere spelers de strijd aan te gaan met monsters die de studenten door de jaren heen mentaal te lijf zijn gegaan. Om alle gebruikers dezelfde informatie te tonen over de status van hun online game,zal er gebruik worden gemaakt van het observer design pattern. De observer design pattern zorgt ervoor dat als er een object van status veranderd, alle afhankelijke objecten hiervan op de hoogte worden gebracht en automatisch worden bijgewerkt. Doormiddel  van de methode ‘INotifyPropertyChanged’ van de ViewModel worden de eigendommen van de objecten bijgewerkt. Voor de communicatie tussen Client en de Server wordt gebruikt gemaakt van SignalR.

* Singleton
Connectie met de server

<h2>C# Threading</h2>

De volgende onderdelen van de code gebruiken threading:

* Async and Await
Webrequests

* Task en Multitasking

Binnen de applicatie en WebAPI wordt gebruik gemaakt van Tasks in een async patroon. Dit komt omdat het SignalR framework geïmplementeerd is, dit 
framework werkt goed samen met Tasks. SignalR kan doormiddel van Tasks garanderen dat een functie uitgevoerd is aan de server of client kant. 
Als bepaalde functies niet het await keyword bevatten dan is het niet mogelijk om te weten of die functie een Exception gegooid heeft. 
De Tasks zullen automatisch gebruik gaan maken van de Thread Pool in .NET. Mochten er functies uitgevoerd moeten worden waar geen Tasks voor beschikbaar 
zijn kunnen deze met de ThreadPool.QueueUserWorkItem functie alsnog op de ThreadPool uitgevoerd worden. 

In de UI vande applicatie zal voor de OnHover events gebruik gemaakt worden van delegates, deze zullen taken uitvoeren die los staan van de UI Thread. 

Server side moet er worden gewacht totdat alle clickbatches van de players ontvangen zijn. Hiervoor wordt een await gebruikt. 
Wanneer alle batches binnen zijn worden deze uitgevoerd en zullen de kliks worden verwerktop de monsters. 

* LINQ/PLINQ

Voor het ophalenen wegschrijven van data wordt gebruik gemaakt van LINQen PLINQ. Player data zal worden weggeschreven en opgehaald met LINQ. 
Naast het Player object worden alle click batches per seconde opgeslagen, hiervoorwordt ook LINQ toegepast. PLINQ wordt gebruikt om de 
batchclicks op te halen en daarna uit te rekenen hoeveel keer er in totaal is geklikt. Om de app compact te houden is ervoor gekozen om environment variables
(images, welke monsters)op te slaan in database. Deze worden gedownload wanneer de app voor het eerst wordt opgestart en zal gebeuren met LINQ.

* Locking
* Delegates
