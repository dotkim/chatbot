class Parse {
  get(message) {
    let regex = /!(\w+)/gi;
    let result = regex.exec(message);
    return result[1];
  }
  
  add(message) {
    let regex = /\+add\ (\w+)\ (.*)/gi
    let result = regex.exec(message);
    return result;
  }

  hue(message) {
    let regex = /\.hue\ (\w+)\ (\w+)/gi
    let result = regex.exec(message);
    return result;
  }
}

module.exports = Parse;  