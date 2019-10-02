const insert = require('../request/insert');
const Get = require('../request/get');

const get = new Get();

module.exports = function (attachments) {
  attachments.forEach(async (attch) => {
    try {
      console.log('attachment found:', attch.id, 'url:', attch.url);
      // get the extension of the file
      let extension = attch.url.split('.').pop();

      const allowedExt = [
        'png',
        'jpg',
        'jpeg',
        'gif'
      ];

      // skip if the extension is longer than 5 characters.
      if (extension.length > 5) return;
      extension = extension.toLowerCase();

      if (!allowedExt.includes(extension)) return;

      // make the extension lowercase
      extension = extension.toLowerCase();

      // create the filename based on message id and the extension
      let name = attch.id + '.' + extension;

      let content = await get.getContent(attch.url);
      insert(name, content.body);
    }
    catch (error) {
      console.error(error);
    }
  });
}