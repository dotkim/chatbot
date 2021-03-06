require('dotenv').config();
const Discord = require('discord.js');
const JsonHandler = require('./data/JsonHandler.js');
const imageHandler = require('./data/imageHandler.js');
const Parse = require('./components/parseMessage.js');
const getRandomInt = require('./components/getRandomInt.js');
const soundCompare = require('./components/keywordCompare.js');
const ImgFetch = require('./components/fetchAttachments.js');
const Get = require('./request/get');
const getRandomImage = require('./request/getRandom');

const client = new Discord.Client();
const data = new JsonHandler();
const parse = new Parse();
const images = new imageHandler();
const get = new Get();

client.on('ready', () => {
  client.user.setPresence({ status: 'online', game: { name: 'v3.5.2 ".help"', type: 'WATCHING' } });
  console.log('bot ready');
  console.log('---------------------');
});

client.on('message', async (message) => {
  try {

    if (message.author.bot) return;

    // thats true
    if (process.env.TRUENUM) {
      thatstrue = getRandomInt(process.env.TRUENUM);
      if (thatstrue === 1) message.reply("That's true.");
    }

    // if there is an attachment save it to a provided Dir.
    console.log('message:', message.id, message.author.username, message.content);
    if (!message.attachments.size == 0) {
      ImgFetch(message.attachments);
    }

    // parse the message content to see if there is an URL in it
    // if there is an URL, check if the content-type is an image and download it
    get.parseImageSource(message.content, message.id);
    
    if (
      (message.content.toLowerCase().includes('donut')) ||
      (message.content.toLowerCase().includes('burger')) ||
      (message.content.toLowerCase().includes('bog collection')) ||
      (message.content.toLowerCase().includes('hotdog')) ||
      (message.content.toLowerCase().includes('bar fightn'))
    ) {
      message.channel.send('byeh');
    }

    if (message.content.toLowerCase().includes('cry')) {
      let obj = await images.cats();
      console.log(obj);
      message.channel.send({
        files: [{
          attachment: String(obj.path),
          name: obj.name
        }]
      });
    }

    if (message.content.toLowerCase().includes('dog')) {
      let obj = await images.dogs();
      console.log(obj);
      message.channel.send({
        files: [{
          attachment: String(obj.path),
          name: obj.name
        }]
      });
    }
    
    if (
      (message.content.toLowerCase().includes('brainlet')) ||
      (message.content.toLowerCase().includes('retard')) ||
      (message.content.toLowerCase().includes('dust')) ||
      (message.content.toLowerCase().includes('stupid'))
      ) {
      let obj = await images.brainlets();
      console.log(obj);
      message.channel.send({
        files: [{
          attachment: String(obj.path),
          name: obj.name
        }]
      });
    }

    if (message.content.toLowerCase().includes('random')) {
      let data = await getRandomImage();
      console.log('sending random image:', data);
      message.channel.send(data);
    }

    // this is where the keywords with ! at the begining is handled.
    if (message.content.startsWith('!')) {
      // get the keywords from the datafile, parse the message for the keyword.

      let keywords = await data.getKeywords();
      let keyword = await parse.get(message.content);
      keyword = keyword.toLowerCase();  // make the keyword lowercase

      if (keywords[keyword] === undefined) {
        console.log('given keyword is undefined, matching soundex');
        // check for similarities using soundex
        // if there is a match just set the keyword to the property from the keywords object.
        keyword = soundCompare(keyword, keywords);
      }

      if (keywords[keyword]) {
        console.log('getting random item');
        let keywordarr = keywords[keyword];         // create an array of the keyword data
        let rand = getRandomInt(keywordarr.length); // get a random int for indexing the array

        message.channel.send(keywordarr[rand]);
      }
    }

    // TODO: make the add keyword take multi line. Seems like my regex can't parse multiline.
    if (message.content.startsWith('+add')) {
      let add = await parse.add(message.content);

      if ((!add[1] || (!add[2]))) {
        message.channel.send('missing keyword param');
        return;
      }

      data.addKeyword(add[1].toLowerCase(), add[2]);

      try {
        message.delete(5000);
      }
      catch (error) {
        console.error(error);
      }

      message.channel.send(add[1] + ' - ' + add[2]);
      data.getKeywords();
    }

    if (message.content.startsWith('+edit')) {
      let edit = await parse.edit(message.content);
      let result = await data.editKeyword(edit[1], edit[2]);
      if (result === 1) message.channel.send('editing: ' + edit[1] + ' new key: ' + edit[2]);
      else if (result === 0) message.channel.send('keywords are alike, did nothing.');
    }

    if (message.content.startsWith('.help')) {
      // give a list of commands.
      let keywords = await data.getKeywords();
      let add = '**+add**\n`+add kodeord tekst`\nLegger til et kodeord for bruk med "!"\n';
      let edit = '**+edit**\n`+edit kodeord endring`\nEndret ett kodeord, første er kodeordet som skal endres, andre er det det skal endres til\n';
      let cmdList = 'Liste over ! kommandoer: ' + Object.keys(keywords).toString().replace(/\,/g, ', '); + '\n';
      message.channel.send(add + edit + cmdList);
    }

  }
  catch (error) {
    console.error(error);
  }
});

client.login(process.env.TOKEN);