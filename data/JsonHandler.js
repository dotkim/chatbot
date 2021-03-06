const fs = require('fs');

class JsonHandler {
  addKeyword(keyword, content) {
    //let data = require('./data.json');
    try {
      fs.readFile('./data/data.json', (err, data) => {
        if (err) throw (err);
        let json = JSON.parse(data);
        if (!json.keywords[keyword]) {
          json.keywords[keyword] = [content];
          fs.writeFileSync('./data/data.json', JSON.stringify(json, 3, 2));
          return keyword, content;
        }
        else {
          let list = json.keywords[keyword];
          list.push(content);
          fs.writeFileSync('./data/data.json', JSON.stringify(json, 3, 2));
          return keyword, content;
        }
      });
    }
    catch (error) {
      console.error(error);
    }
  }

  async getKeywords() {
    // I use fs here instead of requiring the data.
    // Require seems to have some weird memory thing where if its required once, it wont do it again.
    // Yet, the data returned from an already existsing keyword will be included.
    let data = await fs.readFileSync('./data/data.json');
    let json = JSON.parse(data);
    return json.keywords;
  }
  
  async getUser(id) {
    let data = await fs.readFileSync('./data/data.json');
    let json = JSON.parse(data);
    return json.users[id];
  }

  async editKeyword(edit, newWord) {
    let data = await fs.readFileSync('./data/data.json');
    let json = JSON.parse(data);
    if (edit !== newWord) {
      console.log('editing:', edit, 'new key:', newWord);
      Object.defineProperty(json.keywords, newWord,
        Object.getOwnPropertyDescriptor(json.keywords, edit));
      delete json.keywords[edit];
      fs.writeFileSync('./data/data.json', JSON.stringify(json, 3, 2));
      return 1;
    }
    else {
      console.log('keywords are not different');
      return 0;
    }
  }
}

module.exports = JsonHandler;