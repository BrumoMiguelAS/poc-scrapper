# poc-scrapper
Proof of concept of an HTML scrapper using HTML Agility Pack + .NET Core + Hangfire + Mongo DB.

I did this just for fun, to see how the HTML agility pack could be used inside the context of an web application , nevertheless suggestions and improvements are welcomed.

The UI doenst work, we have to use some HTTP client (i use Postman), and im also experimenting adding some kind of image recognizer like Google or Microsoft one in the media processing phase.

* Download the example and open it on visual studio
* Run it
* Open postman and just add a scrapping source to your databse using:

```javascript
POST - http://[YOUR_DOMAIN]/scrapping/sources
BODY:
{
  Name: "Name_of_the_source",
	Url: "Url_of_the_source"
}

```
* With the returned Id, just use:

```javascript
POST - http://[YOUR_DOMAIN]/scrapping/sources/[ID]/schedule
BODY:
{
}

```
* This will enqueue an Hangfire Job that will crawl the website and download all the images to a folder inside Presentation.Web folder

* Thats all, for now!
