require('dotenv').config();
const fs = require('fs');
const getRandomInt = require('../components/getRandomInt.js');

class imageHander {
  async cats() {
    let files = await fs.readdirSync(process.env.CATDIR)
    let rand = getRandomInt(files.length);
    let img = files[rand];
    return { path: process.env.CATDIR + img, name: img };
  }
  
  async brainlets() {
    let files = await fs.readdirSync(process.env.BRDIR)
    let rand = getRandomInt(files.length);
    let img = files[rand];
    return { path: process.env.BRDIR + img, name: img };
  }

  async dogs() {
    let files = await fs.readdirSync(process.env.DOGDIR)
    let rand = getRandomInt(files.length);
    let img = files[rand];
    return { path: process.env.DOGDIR + img, name: img };
  }
}


module.exports = imageHander;