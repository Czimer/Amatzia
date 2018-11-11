var ACCESS_TOKEN = 'EAAfx6XTSOA8BAKHSBjJCdXDmijqoKKVpAMtASsR5ZAlyEAAfx6XTSOA8BAF4devTI7PQjMJB2QyuZAZCtYTzWpXZBdZB1YcZAMr11X3H1aXytcnk0msUHrjZCcsZCEchKZBfCEfGINuq8ibWaQi33YiZBXwjxZBp8WWX0L4wellSXFGFmZBKdU0bXiuSRQbvYa4t6dg4F3XAnAGquESQRFUhZAGC1Y9CWQUgQ74wcWYf6Lv6jOOo3wLnoKDwkuPOBOHJUHgJ63AEjVgfwDWk93CcDNlyl6jijOFLw5ZAMcnDw4I6S9ln1SfLXEH5cSThIf6hnHNkT7ApKMHy5w2EeqwXZBegqDipx3IImPRrXvkHU3NqhHfNVcFRmqbWs8ZBkbDfa39ZBzD1lONLCYxlcvVKvJFhIXcGizbjfH4Ooq404qdL6A16fgLQyGYqSkXmae58DZC5eZABmQtjHoRm';
var message = "";
window.fbAsyncInit = function () {
    FB.init({
        appId: '2236309826582543',
      autoLogAppEvents : true,
      xfbml            : true,
      version          : 'v3.2'
    });

    getBlaBla();
    shareButton()
  };

  (function(d, s, id){
     var js, fjs = d.getElementsByTagName(s)[0];
     if (d.getElementById(id)) {return;}
     js = d.createElement(s); js.id = id;
     js.src = "https://connect.facebook.net/en_US/sdk.js";
     fjs.parentNode.insertBefore(js, fjs);
   }(document, 'script', 'facebook-jssdk'));

var getBlaBla = function () {
    FB.api(
        '/1487685995317/photos?fields=images&access_token=' + ACCESS_TOKEN,
        'GET',
        {},
        function (response) {
            var photos = response.data;

            for (var photo of photos) {
                var lastUrlIndex = photo.images.length - 1;
                var imageSrc = photo.images[lastUrlIndex].source;
                var img = document.createElement("img");
                img.src = imageSrc;
                document.body.appendChild(img);
            }
        }
    );
} 

var facebookPostFeed = function () {
    FB.api(
        '/1879918278711452/feed',
        'POST',
        { "message": message},
        function (response) {
            // Insert your code here
        }
    );
}

    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = 'https://connect.facebook.net/en_US/sdk.js#xfbml=1&version=v3.2&appId=2236309826582543&autoLogAppEvents=1';
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));