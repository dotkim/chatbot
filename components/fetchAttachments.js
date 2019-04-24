require('dotenv').config();
const https = require('https');
const fs = require('fs');

module.exports = function attchFetch(attachments) {
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