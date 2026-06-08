using DeThiThu.Contexts;
using DeThiThu.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeThiThu.Services;

public class DataSeeder : IDataSeeder
{
	private readonly CandyContext _context;

	public DataSeeder(CandyContext context)
	{
		_context = context;
	}

	public async Task ImportAsync(CancellationToken cancellationToken = default)
	{
		await _context.Database.EnsureCreatedAsync(cancellationToken);

		if (!await _context.Categories.AnyAsync(cancellationToken))
		{
			var names = new List<string>()
			{
				"Bulk Candy", "Bite Size", "Bon Bons", 
				"Bubble Gum", "Wedding", "Coated Popcorn",
				"Chocolates", "Caramels", "Candy Toys", "Fizzies"
			};
			_context.Categories.AddRange(names.Select(x => new Category()
			{
				Name = x, ShowOnMenu = false
			}));
			await _context.SaveChangesAsync(cancellationToken);
		}

		var categoryIdList = await _context.Categories
			.Select(x => x.Id)
			.ToListAsync(cancellationToken);

		if (!await _context.Candies.AnyAsync(cancellationToken))
		{
			var candies = new[]
			{
				"Hershey's Kisses White Foiled Milk Chocolate Candy: 400-Piece Bag",
				"Hershey's Kisses Red Foiled Milk Chocolate Candy: 400-Piece Bag",
				"Hershey's Kisses Light Blue Foiled Milk Chocolate Candy: 400-Piece Bag",
				"Jelly Belly 49 Flavors Jelly Beans: 2LB Bag", 
				"Foiled Caramel Candy - Light Blue: 180-Piece Bag",
				"Hershey's Kisses Silver Foiled Milk Chocolate Candy: 56-Ounce Bag",
				"Hershey's Kisses Dark Green Foiled Milk Chocolate Candy: 400-Piece Bag",
				"Foiled Caramel Candy - White: 180-Piece Bag", 
				"Foiled Caramel Candy - Blue: 180-Piece Bag",
				"Tesla's Tremendously Tall 3-Ounce Twist Pops - Strawberry: 12-Piece Box",
				"Foiled Caramel Candy - Black: 180-Piece Bag",
				"Dum Dums Royal Blue Party Pops - Blueberry: 75-Piece Bag",
				"Pufflettes Gummy Bites - Blue Raspberry: 1KG Bag",
				"Old Fashioned Hard Candy Sticks - Peppermint: 80-Piece Box",
				"Hershey's Kisses Kiwi Green Foiled Milk Chocolate Candy: 400-Piece Bag",
				"Foiled Caramel Candy - Silver: 180-Piece Bag",
				"YumJunkie Sassy Straws Candy Powder Filled Mini Straws - Watermelon: 50-Piece Bag",
				"Jelly Belly Black Licorice: 2LB Bag", 
				"Jelly Belly Blueberry: 2LB Bag",
				"Ring Pop Assorted Candy: 44-Piece Tub", 
				"Jelly Belly Bubblegum: 2LB Bag",
				"Jelly Belly Very Cherry: 2LB Bag", 
				"Dove Dark Chocolate Peanut Butter Squares: 28-Piece Bag",
				"CurlyCutes Petite Crystal Ribbon Pops - Orange: 20-Piece Jar",
				"Dum Dums Rainbow Party Pops - Assorted Flavors: 75-Piece Bag",
				"Old Fashioned Hard Candy Sticks - Cinnamon: 80-Piece Box", 
				"Jelly Belly Berry Blue: 2LB Bag",
				"Jelly Belly Cotton Candy: 2LB Bag", 
				"Jelly Belly French Vanilla: 2LB Bag",
				"Dum Dums Green Party Pops - Sour Apple: 75-Piece Bag",
				"Nautilus Belgian Chocolate Seashells: 18-Piece Box",
				"YumJunkie Sassy Straws Candy Powder Filled Mini Straws - Black Cherry: 50-Piece Bag",
				"Dum Dums Hot Pink Party Pops - Watermelon: 75-Piece Bag",
				"Old Fashioned Hard Candy Sticks - Root Beer: 80-Piece Box",
				"YumJunkie Sassy Straws Candy Powder Filled Mini Straws - Strawberry: 50-Piece Bag",
				"Dum Dums Light Blue Party Pops - Blu Raspberry: 75-Piece Bag",
				"Anthon Berg Chocolate Cocktails Liquor Bottles: 4-Piece Box", 
				"Jelly Belly Coconut: 2LB Bag",
				"Petite Pufflettes Gummy Bites - Orange: 5LB Bag", 
				"Dum Dums Red Party Pops - Strawberry: 75-Piece Bag",
				"YumJunkie Sassy Spheres Cherry Black Striped Ball Lollipops - Petite: 400-Piece Bag",
				"Espeez Rock Candy Crystal Sticks - White: 36-Piece Tub",
				"Anthon Berg Chocolate Liquor Bottles: 4-Piece Box", 
				"Jelly Belly Jewel: 2LB Bag",
				"YumJunkie Mini White Candy Crosses: 5LB Bag",
				"Old Fashioned Hard Candy Sticks - Watermelon: 80-Piece Box",
				"Old Fashioned Hard Candy Sticks - Butterscotch: 80-Piece Box",
				"Old Fashioned Hard Candy Sticks - Lemon: 80-Piece Box"
			};

			var today = DateTime.Now.Date;

			_context.Candies.AddRange(candies.Select(x => new Candy()
			{
				Name = x,
				Price = Random.Shared.Next(500, 20000) / 100m,
				ExpirationDate = today.AddDays(Random.Shared.Next(100, 600)),
				CategoryId = categoryIdList[Random.Shared.Next(categoryIdList.Count)]
			}));

			await _context.SaveChangesAsync(cancellationToken);
		}

		if (!await _context.Subscribers.AnyAsync(cancellationToken))
		{
			var subscribers = new List<Subscriber>()
			{
				new() { Email = "alexthankful@yahoo.ca", SubscribedDate = new DateTime(2020, 4, 24, 9, 29, 31)},
				new() { Email = "jasonenthusiastic@live.nl", SubscribedDate = new DateTime(2013, 2, 6, 10, 54, 27)},
				new() { Email = "inquisitivemartin@chello.nl", SubscribedDate = new DateTime(2018, 3, 3, 20, 1, 42)},
				new() { Email = "ashleighlong@yahoo.it", SubscribedDate = new DateTime(2010, 10, 4, 2, 35, 34)},
				new() { Email = "tastywesley96@ntlworld.com", SubscribedDate = new DateTime(2012, 11, 11, 5, 19, 52)},
				new() { Email = "anxiousricardo80@tiscali.it", SubscribedDate = new DateTime(2010, 10, 15, 21, 30, 43)},
				new() { Email = "eddielonely@hotmail.com", SubscribedDate = new DateTime(2013, 9, 21, 8, 20, 38)},
				new() { Email = "drabmarisa@uol.com.br", SubscribedDate = new DateTime(2020, 8, 26, 16, 15, 22)},
				new() { Email = "luisbusy@chello.nl", SubscribedDate = new DateTime(2010, 4, 17, 19, 13, 12)},
				new() { Email = "glenncharming@virgilio.it", SubscribedDate = new DateTime(2023, 1, 2, 3, 29, 2)},
				new() { Email = "nuttybridget@blueyonder.co.uk", SubscribedDate = new DateTime(2014, 11, 13, 12, 19, 22)},
				new() { Email = "repulsivebryan79@qq.com", SubscribedDate = new DateTime(2022, 4, 25, 23, 59, 7)},
				new() { Email = "thoughtlessalan71@tiscali.co.uk", SubscribedDate = new DateTime(2021, 2, 16, 18, 6, 33)},
				new() { Email = "perfectlatoya2@hetnet.nl", SubscribedDate = new DateTime(2012, 1, 5, 12, 37, 38)},
				new() { Email = "fairomar@mail.com", SubscribedDate = new DateTime(2021, 10, 28, 21, 23, 42)},
				new() { Email = "finevanessa25@hotmail.es", SubscribedDate = new DateTime(2013, 11, 24, 9, 29, 24)},
				new() { Email = "motionlesskristopher@libero.it", SubscribedDate = new DateTime(2013, 2, 16, 12, 0, 8)},
				new() { Email = "outstandingchristy72@hotmail.com", SubscribedDate = new DateTime(2010, 3, 6, 15, 23, 2)},
				new() { Email = "jenniferwitty@arcor.de", SubscribedDate = new DateTime(2015, 3, 27, 23, 2, 12)},
				new() { Email = "robinwicked@comcast.net", SubscribedDate = new DateTime(2020, 12, 6, 11, 18, 5)},
				new() { Email = "garyrelieved@live.com.au", SubscribedDate = new DateTime(2013, 8, 7, 11, 50, 4)},
				new() { Email = "gloriouskristin12@rocketmail.com", SubscribedDate = new DateTime(2018, 1, 21, 2, 41, 27)},
				new() { Email = "cooperativehenry@verizon.net", SubscribedDate = new DateTime(2021, 10, 14, 7, 23, 19)},
				new() { Email = "cloudysean24@bellsouth.net", SubscribedDate = new DateTime(2021, 10, 3, 4, 55, 37)},
				new() { Email = "finemelanie72@chello.nl", SubscribedDate = new DateTime(2012, 9, 2, 18, 27, 24)},
				new() { Email = "shyalejandro9@hotmail.co.uk", SubscribedDate = new DateTime(2016, 1, 12, 2, 31, 51)},
				new() { Email = "fierceraquel@yahoo.com.br", SubscribedDate = new DateTime(2010, 4, 13, 9, 48, 36)},
				new() { Email = "beautifuldonna89@mail.com", SubscribedDate = new DateTime(2022, 2, 18, 3, 48, 30)},
				new() { Email = "cautiousjacqueline19@live.com.au", SubscribedDate = new DateTime(2022, 12, 8, 19, 45, 37)},
				new() { Email = "arrogantlauren@optonline.net", SubscribedDate = new DateTime(2010, 3, 22, 19, 10, 26)},
				new() { Email = "lancedifficult@juno.com", SubscribedDate = new DateTime(2016, 3, 2, 23, 20, 10)},
				new() { Email = "larryinnocent@hotmail.es", SubscribedDate = new DateTime(2022, 2, 14, 10, 17, 32)},
				new() { Email = "sillynikki@yahoo.co.uk", SubscribedDate = new DateTime(2010, 12, 9, 19, 42, 48)},
				new() { Email = "colorfuljanelle53@neuf.fr", SubscribedDate = new DateTime(2013, 10, 22, 14, 24, 40)},
				new() { Email = "ashamedpatrick@centurytel.net", SubscribedDate = new DateTime(2016, 4, 24, 10, 15, 33)},
				new() { Email = "scottrepulsive@tiscali.it", SubscribedDate = new DateTime(2023, 6, 7, 1, 5, 42)},
				new() { Email = "handsometony@windstream.net", SubscribedDate = new DateTime(2020, 8, 23, 10, 3, 10)},
				new() { Email = "foolishreginald@juno.com", SubscribedDate = new DateTime(2010, 3, 10, 18, 47, 25)},
				new() { Email = "spotlessvictor40@live.com.au", SubscribedDate = new DateTime(2018, 10, 5, 7, 18, 14)},
				new() { Email = "encouragingshawn@orange.fr", SubscribedDate = new DateTime(2023, 2, 11, 7, 42, 12)},
				new() { Email = "uptightsabrina@mail.com", SubscribedDate = new DateTime(2023, 5, 12, 16, 27, 56)},
				new() { Email = "karavivacious@bigpond.net.au", SubscribedDate = new DateTime(2018, 2, 21, 19, 46, 0)},
				new() { Email = "fantasticmegan98@sbcglobal.net", SubscribedDate = new DateTime(2021, 3, 25, 13, 54, 32)},
				new() { Email = "fiercecandice@yahoo.fr", SubscribedDate = new DateTime(2013, 6, 30, 6, 4, 25)},
				new() { Email = "brianimpossible@yahoo.com.mx", SubscribedDate = new DateTime(2021, 9, 28, 13, 42, 27)},
				new() { Email = "cruelbrian31@live.ca", SubscribedDate = new DateTime(2020, 11, 11, 8, 46, 15)},
				new() { Email = "lisacheerful@sky.com", SubscribedDate = new DateTime(2011, 7, 14, 20, 23, 36)},
				new() { Email = "wanderingmicah10@laposte.net", SubscribedDate = new DateTime(2022, 12, 13, 17, 8, 42)},
				new() { Email = "drewwrong@aim.com", SubscribedDate = new DateTime(2019, 6, 10, 1, 13, 55)},
				new() { Email = "repulsivetara@yahoo.com.br", SubscribedDate = new DateTime(2015, 7, 3, 11, 5, 34)},
				new() { Email = "jessicaglorious@rambler.ru", SubscribedDate = new DateTime(2015, 8, 7, 14, 20, 1)},
				new() { Email = "defiantjeffrey@chello.nl", SubscribedDate = new DateTime(2011, 7, 27, 2, 51, 40)},
				new() { Email = "gabrieldepressed@home.nl", SubscribedDate = new DateTime(2012, 3, 3, 15, 17, 35)},
				new() { Email = "hungryamber@sbcglobal.net", SubscribedDate = new DateTime(2022, 5, 9, 8, 39, 21)},
				new() { Email = "vastlarry20@shaw.ca", SubscribedDate = new DateTime(2016, 5, 14, 9, 9, 49)},
				new() { Email = "difficultgregory74@neuf.fr", SubscribedDate = new DateTime(2011, 3, 8, 12, 3, 57)},
				new() { Email = "cameronwitty@live.it", SubscribedDate = new DateTime(2019, 12, 18, 6, 37, 16)},
				new() { Email = "enviouspaula@club-internet.fr", SubscribedDate = new DateTime(2011, 11, 8, 1, 32, 50)},
				new() { Email = "mauricehelpful@live.it", SubscribedDate = new DateTime(2010, 12, 1, 16, 39, 12)},
				new() { Email = "obnoxiousjessica0@frontiernet.net", SubscribedDate = new DateTime(2015, 12, 28, 5, 5, 42)},
				new() { Email = "homelydeborah@live.co.uk", SubscribedDate = new DateTime(2020, 5, 12, 22, 29, 15)},
				new() { Email = "angelajittery@charter.net", SubscribedDate = new DateTime(2010, 9, 11, 11, 11, 18)},
				new() { Email = "moderntheresa4@mail.com", SubscribedDate = new DateTime(2014, 1, 11, 22, 41, 14)},
				new() { Email = "gleamingtheodore@hotmail.co.uk", SubscribedDate = new DateTime(2017, 9, 25, 7, 19, 44)},
				new() { Email = "healthymario46@aim.com", SubscribedDate = new DateTime(2020, 2, 7, 15, 12, 43)},
				new() { Email = "embarrasseddouglas@centurytel.net", SubscribedDate = new DateTime(2018, 5, 24, 7, 35, 32)},
				new() { Email = "peterdepressed@sfr.fr", SubscribedDate = new DateTime(2012, 2, 1, 2, 9, 16)},
				new() { Email = "aprilgentle@orange.fr", SubscribedDate = new DateTime(2015, 7, 23, 16, 59, 11)},
				new() { Email = "giftedjohnny@libero.it", SubscribedDate = new DateTime(2016, 2, 1, 2, 8, 28)},
				new() { Email = "glamorousjerome@sbcglobal.net", SubscribedDate = new DateTime(2016, 10, 19, 21, 12, 20)},
				new() { Email = "cleversean61@charter.net", SubscribedDate = new DateTime(2010, 3, 10, 4, 10, 52)},
				new() { Email = "gloriousmallory@qq.com", SubscribedDate = new DateTime(2021, 6, 4, 6, 10, 7)},
				new() { Email = "naughtywillie@optusnet.com.au", SubscribedDate = new DateTime(2016, 7, 1, 9, 35, 8)},
				new() { Email = "charmingmolly96@hotmail.es", SubscribedDate = new DateTime(2014, 4, 8, 12, 42, 12)},
				new() { Email = "impossibleadrienne@live.co.uk", SubscribedDate = new DateTime(2023, 12, 1, 13, 26, 8)},
				new() { Email = "kennethdetermined@att.net", SubscribedDate = new DateTime(2020, 10, 29, 18, 56, 4)},
				new() { Email = "foolishregina35@rocketmail.com", SubscribedDate = new DateTime(2020, 4, 19, 5, 15, 54)},
				new() { Email = "robynexpensive@yahoo.com.mx", SubscribedDate = new DateTime(2015, 3, 21, 6, 58, 51)},
				new() { Email = "wesleybetter@alice.it", SubscribedDate = new DateTime(2017, 2, 23, 12, 44, 5)},
				new() { Email = "mistybryce@sympatico.ca", SubscribedDate = new DateTime(2017, 10, 23, 20, 29, 27)},
				new() { Email = "emmanuelblue-eyed@yahoo.it", SubscribedDate = new DateTime(2022, 10, 12, 12, 16, 22)},
				new() { Email = "putridjasmine19@cox.net", SubscribedDate = new DateTime(2018, 8, 9, 13, 59, 21)},
				new() { Email = "selfishrebekah3@hotmail.es", SubscribedDate = new DateTime(2017, 1, 22, 23, 4, 34)},
				new() { Email = "thankfulmelanie91@centurytel.net", SubscribedDate = new DateTime(2011, 8, 28, 19, 18, 0)},
				new() { Email = "amusedsean@bluewin.ch", SubscribedDate = new DateTime(2013, 4, 10, 15, 55, 0)},
				new() { Email = "jealousbrandi@earthlink.net", SubscribedDate = new DateTime(2018, 1, 26, 7, 35, 31)},
				new() { Email = "differentjulian@juno.com", SubscribedDate = new DateTime(2017, 3, 23, 21, 28, 19)},
				new() { Email = "armandotame@yahoo.ca", SubscribedDate = new DateTime(2023, 2, 3, 2, 55, 25)},
				new() { Email = "enchantingleonard@hotmail.com", SubscribedDate = new DateTime(2013, 5, 16, 22, 9, 7)},
				new() { Email = "innocentbethany16@facebook.com", SubscribedDate = new DateTime(2020, 12, 1, 23, 29, 34)},
				new() { Email = "darkscott12@tin.it", SubscribedDate = new DateTime(2021, 9, 22, 9, 42, 21)},
				new() { Email = "oddmeredith@me.com", SubscribedDate = new DateTime(2017, 4, 19, 22, 14, 15)},
				new() { Email = "depressedjulia@libero.it", SubscribedDate = new DateTime(2020, 2, 17, 15, 31, 3)},
				new() { Email = "benjaminencouraging@gmx.de", SubscribedDate = new DateTime(2020, 9, 21, 11, 14, 46)},
				new() { Email = "tastytracy@facebook.com", SubscribedDate = new DateTime(2017, 11, 11, 12, 30, 25)},
				new() { Email = "curiousaimee@hotmail.es", SubscribedDate = new DateTime(2021, 11, 16, 15, 13, 31)},
				new() { Email = "nuttyanne@optusnet.com.au", SubscribedDate = new DateTime(2018, 1, 26, 23, 21, 10)},
				new() { Email = "sethrepulsive@hotmail.de", SubscribedDate = new DateTime(2018, 7, 8, 10, 56, 30)},
				new() { Email = "meredithodd@aim.com", SubscribedDate = new DateTime(2018, 7, 2, 12, 32, 14)},
				new() { Email = "aarontired@laposte.net", SubscribedDate = new DateTime(2019, 11, 13, 7, 45, 25)}
			};

			_context.Subscribers.AddRange(subscribers);

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}