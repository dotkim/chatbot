require('dotenv').config();
const imageThumbnail = require('image-thumbnail');
const fs = require('fs');

const path = process.env.FILEPATH;
const thumbpath = process.env.THUMBPATH;
let options = { width: 200 }

module.exports = async function (name) {
  try {
    let file = path + name;
    let thumbfile = thumbpath + name;
    const thumbnail = await imageThumbnail(file, options);
    fs.writeFileSync(thumbfile, thumbnail);
  }
  catch (error) {
    console.error(error);
  }
}