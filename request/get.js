const insert = require('./insert');

module.exports = class {
  async parseImageSource(content, id) {
    // regex for parsing a URL
    let regex = /((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/;
    // split the message by spaces
    // since there isnt any spaces in the url we can capture it this way.
    let contentArr = content.split(' ');

    // check if any of the provided content is an URL.
    contentArr.forEach(async (str) => {

      let url = regex.exec(str);
      if (url) {
        console.log('url found:', url[1]);
        // check if my domain CNAME is contained in the message
        if (url[1].includes(process.env.CNAME)) return;

        const allowedExt = [
          'png',
          'jpg',
          'jpeg',
          'gif'
        ];

        let extension = url[1].split('.').pop();  // get the extension of the file
        if (extension.length > 5) return;         // skip if the extension is longer than 5 characters.
        extension = extension.toLowerCase();

        if (!allowedExt.includes(extension)) return;

        // create path and filename.
        let name = id + '.' + extension;
        let imgRegex = /^(image|video)\/\w/gi;

        try {
          let b64Content = await this.getContent(url[1]);

          if (!imgRegex.exec(b64Content.contentType)) return;
          insert(name, b64Content['body']);
        }
        catch (error) {
          console.error(error);
        }
      }
    });
  }

  async getContent(url) {
    // return new pending promise
    return new Promise((resolve, reject) => {
      // select http or https module, depending on reqested url
      const lib = url.startsWith('https') ? require('https') : require('http');
      const request = lib.get(url, (response) => {
        // handle http errors
        if (response.statusCode < 200 || response.statusCode > 299) {
          reject(new Error('Failed to load page, status code: ' + response.statusCode));
        }

        let type = response.headers['content-type'];
        // set encoding to base 64
        response.setEncoding('base64');
        // temporary data holder
        const body = [];
        // on every content chunk, push it to the data array
        response.on('data', (chunk) => body.push(chunk));
        // we are done, resolve promise with those joined chunks
        response.on('end', () => resolve({ body: body.join(''), contentType: type }));
      });
      // handle connection errors of the request
      request.on('error', (err) => reject(err))
    })
  };
} 