require('dotenv').config();
const https = require('https');
const http = require('http');
const fs = require('fs');
const thumbnail = require('./thumbnail');

function publishImage(name) {
  // this function publishes an image to the web api "images"
  let url = process.env.APIURL + name;  // create the URL
  let req = http.request(url, { method: 'POST' }, function(res) {
    console.log('response status:', res.statusCode);
  });
  req.end();
}

class Fetch {
  attchFetch(attachments) {
    attachments.forEach((attch) => {
      try {
        console.log('attachment found:', attch.id, 'url:', attch.url);
        let extension = attch.url.split('.').pop(); // get the extension of the file
        extension = extension.toLowerCase();        // make the extension lowercase
        let name = attch.id + '.' + extension;      // create the filename based on message id and the extension
        let path = process.env.FILEPATH + name;     // create the path for storing the image.
        let file = fs.createWriteStream(path);      // create a write stream to write the image buffer to
        https.get(attch.url, function (response) {
          response.pipe(file);  // pipe the request buffer to the stream
        });
        // use timeout, i dont like it but, its what works when i create thumbnails
        setTimeout(function () { thumbnail(name) }, 1000);
        publishImage(name);
      }
      catch (error) {
        console.error(error);
      }
    });
  }

  urlattch(content, id) {
    // regex for parsing a URL
    let regex = /((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/;
    // split the message by spaces
    // since there isnt any spaces in the url we can capture it this way.
    let contentArr = content.split(' ');

    // check if any of the provided content is an URL.
    contentArr.forEach((str) => {

      let url = regex.exec(str);
      if (url) {
        console.log('url found:', url[1]);
        // check if my domain CNAME is contained in the message
        if (url[1].includes(process.env.CNAME)) return;
        let extension = url[1].split('.').pop();  // get the extension of the file
        if (extension.length > 5) return;         // skip if the extension is longer than 5 characters.
        extension = extension.toLowerCase();

        // create path and filename.
        let name = id + '.' + extension;
        let path = process.env.FILEPATH + name;
        let imgRegex = /^(image|video)\/\w/gi;

        if (url[3] === 'https://') {
          try {
            https.get(url[1], function (response) {
              // check if the content-type is of video or image.
              let img = imgRegex.exec(response.headers['content-type']);
              if (!img) return;

              // create filestream and pipe response buffer to file.
              let file = fs.createWriteStream(path);
              response.pipe(file);
            });
            setTimeout(function () { thumbnail(name) }, 1000);
            publishImage(name);
          }
          catch (error) {
            console.error(error);
          }
        }

        else if (url[3] === 'http://') {
          try {
            http.get(url[1], function (response) {
              // check if the content-type is of video or image.
              let img = imgRegex.exec(response.headers['content-type']);
              if (!img) return;

              // create filestream and pipe response buffer to file.
              let file = fs.createWriteStream(path);
              response.pipe(file);
            });
            setTimeout(function () { thumbnail(name) }, 1000);
            publishImage(name);
          }
          catch (error) {
            console.error(error);
          }
        }
      }
    });
  }
}

module.exports = Fetch;