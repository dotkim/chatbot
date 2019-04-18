const fs = require('fs');

class JsonHandler {
  addKeyword(keyword, content) {
    //let data = require('./data.json');
    try {
      fs.readFile('./data/data.json', (err, data) => {
        if (err) throw(err);
        let json = JSON.parse(data);
        if (!json.keywords[keyword]) {
          json.keywords[keyword] = [content];
          fs.writeFileSync('./data/data.json', JSON.stringify(json, 3, 1));
          return keyword, content;
        }
        else {
          let list = json.keywords[keyword];
          list.push(content);
          fs.writeFileSync('./data/data.json', JSON.stringify(json, 3, 1));
          return keyword, content;
        }  
      });
    }
    catch (error) {
      console.error(error);
    }
  }

  getKeywords() {
    let data = require('./data.json');
    return data.keywords;
  }
}

module.exports = JsonHandler;