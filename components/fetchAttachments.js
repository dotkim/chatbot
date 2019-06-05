require('dotenv').config();
const https = require('https');
const http = require('http');
const fs = require('fs');
const thumbnail = require('./thumbnail');

function publishImage(name) {
  let url = process.env.APIURL + name;
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
        let extension = attch.url.split('.').pop();
        extension = extension.toLowerCase();
        let name = attch.id + '.' + extension;
        let path = process.env.FILEPATH + attch.id + '.' + extension;
        let file = fs.createWriteStream(path);
        https.get(attch.url, function (response) {
          response.pipe(file);
        });
        setTimeout(function () { thumbnail(name) }, 1000);
        publishImage(name);
      }
      catch (error) {
        console.error(error);
      }
    });
  }

  urlattch(content, id) {
    let regex = /((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/;
    let contentArr = content.split(' ');

    // check if any of the provided content is an URL.
    contentArr.forEach((str) => {

      let url = regex.exec(str);
      if (url) {
        console.log('url found:', url[1]);
        let extension = url[1].split('.').pop();
        extension = extension.toLowerCase();
        let name = id + '.' + extension;
        let path = process.env.FILEPATH + id + '.' + extension;
        let imgRegex = /^(image|video)\/\w/gi;

        if (url[3] === 'https://') {
          try {
            https.get(url[1], function (response) {
              let img = imgRegex.exec(response.headers['content-type']);
              if (!img) return;

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
              let img = imgRegex.exec(response.headers['content-type']);
              if (!img) return;

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