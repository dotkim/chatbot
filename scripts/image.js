/* eslint-disable */
const ImageFactory = require('../src/lib/imageFactory');
const factory = new ImageFactory();

(async function() {
  let keyword = await factory.get();
  console.log(`Received object: ${keyword.schema}\nKeyword: ${keyword.schema.fileName}\nValues: ${keyword.schema.url}`);
})();