require('dotenv').config();
const https = require('https');
const http = require('http');
const fs = require('fs');

class Fetch {
  attchFetch(attachments) {
    attachments.forEach((attch) => {
      console.log('attachment found:', attch.id, 'url:', attch.url);
      let extension = attch.url.split('.').pop();
      let path = process.env.FILEPATH + attch.id + '.' + extension;
      let file = fs.createWriteStream(path);
      https.get(attch.url, function (response) {
        response.pipe(file);
      });
    });
  }

  urlattch(content, id) {
    let regex = /((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/;
    let contentArr = content.split(' ');
    
    // check if any of the provided content is an URL.
    contentArr.forEach((str) => {
      
      let url = regex.exec(str);
      if (url) {
        
        let extension = url[1].split('.').pop();
        let path = process.env.FILEPATH + id + '.' + extension;
        console.log('path:',path);
        
        if (url[3] === 'https://') {
          https.get(url[1], function (response) {
            console.log('headers:', response.headers['content-type']);
            let imgRegex = /(image)\/\w/gi;
            let img = imgRegex.exec(response.headers['content-type']);
            console.log('img:', img);
            if (!img) return;

            let file = fs.createWriteStream(path);
            response.pipe(file);
          });
        }
        else if (url[3] === 'http://') {
          http.get(url[1], function (response) {
            console.log('headers:', response.headers['content-type']);
            let imgRegex = /(image)\/\w/gi;
            let img = imgRegex.exec(response.headers['content-type']);
            console.log('img:', img);
            if (!img) return;

            let file = fs.createWriteStream(path);
            response.pipe(file);
          });
        }
      }
    });
  }

}

module.exports = Fetch;