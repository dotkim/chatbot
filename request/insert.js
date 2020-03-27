require('dotenv').config();
const http = require('http');

function bodyObject(name, content) {
  return JSON.stringify({ name: name, imageData: content });
}

module.exports = function (name, content) {
  const auth = new Buffer.from(process.env.AUTH).toString('base64');
  
  let body = bodyObject(name, content);

  let opt = {
    hostname: process.env.APIURL,
    path: '/images',
    port: process.env.APIPORT,
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Content-Length': Buffer.byteLength(body),
      'Authorization': 'Basic ' + auth
    }
  };

  let req = http.request(opt, function (res) {
    console.log(`STATUS: ${res.statusCode}`);
    console.log(`HEADERS: ${JSON.stringify(res.headers)}`);
  });
  req.on('error', function (error) {
    console.error(error);
  });

  req.write(body);
  req.end();
}