require('dotenv').config();
const fs = require('fs');
const getRandomInt = require('../components/getRandomInt.js');

async function cats() {
  let files = await fs.readdirSync(process.env.IMGDIR)
  let rand = getRandomInt(files.length);
  let img = files[rand];
  return { path: process.env.IMGDIR + img, name: img };
}

module.exports = cats;