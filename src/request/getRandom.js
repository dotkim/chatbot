module.exports = async function () {
  try {
    const data = await getImageUrl();
    return data.url;
  }
  catch (err) {
    console.error(err);
    return;
  }
}


async function getImageUrl() {
  return new Promise((resolve, reject) => {
    const url = 'http://api.knerli.no/images/getRandom';
    // select http or https module, depending on reqested url
    const lib = url.startsWith('https') ? require('https') : require('http');
    const request = lib.get(url, (response) => {
      // handle http errors
      if (response.statusCode < 200 || response.statusCode > 299) {
        reject(new Error('Failed to load page, status code: ' + response.statusCode));
      }
      const body = [];
      response.on('data', (chunk) => body.push(chunk));
      // we are done, resolve promise with those joined chunks
      response.on('end', () => resolve(JSON.parse(body.join(''))));
    });
    // handle connection errors of the request
    request.on('error', (err) => reject(err))
  });
}