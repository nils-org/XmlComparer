
var camelCaseTokenizer = function (builder) {

  var pipelineFunction = function (token) {
    var previous = '';
    // split camelCaseString to on each word and combined words
    // e.g. camelCaseTokenizer -> ['camel', 'case', 'camelcase', 'tokenizer', 'camelcasetokenizer']
    var tokenStrings = token.toString().trim().split(/[\s\-]+|(?=[A-Z])/).reduce(function(acc, cur) {
      var current = cur.toLowerCase();
      if (acc.length === 0) {
        previous = current;
        return acc.concat(current);
      }
      previous = previous.concat(current);
      return acc.concat([current, previous]);
    }, []);

    // return token for each string
    // will copy any metadata on input token
    return tokenStrings.map(function(tokenString) {
      return token.clone(function(str) {
        return tokenString;
      })
    });
  }

  lunr.Pipeline.registerFunction(pipelineFunction, 'camelCaseTokenizer')

  builder.pipeline.before(lunr.stemmer, pipelineFunction)
}
var searchModule = function() {
    var documents = [];
    var idMap = [];
    function a(a,b) { 
        documents.push(a);
        idMap.push(b); 
    }

    a(
        {
            id:0,
            title:"UncomparableNodeException",
            content:"UncomparableNodeException",
            description:'',
            tags:''
        },
        {
            url:'/XmlComparer/api/XmlComparer/UncomparableNodeException',
            title:"UncomparableNodeException",
            description:""
        }
    );
    a(
        {
            id:1,
            title:"XmlDifference",
            content:"XmlDifference",
            description:'',
            tags:''
        },
        {
            url:'/XmlComparer/api/XmlComparer/XmlDifference',
            title:"XmlDifference",
            description:""
        }
    );
    a(
        {
            id:2,
            title:"Comparer",
            content:"Comparer",
            description:'',
            tags:''
        },
        {
            url:'/XmlComparer/api/XmlComparer/Comparer',
            title:"Comparer",
            description:""
        }
    );
    a(
        {
            id:3,
            title:"DifferenceSource",
            content:"DifferenceSource",
            description:'',
            tags:''
        },
        {
            url:'/XmlComparer/api/XmlComparer/DifferenceSource',
            title:"DifferenceSource",
            description:""
        }
    );
    a(
        {
            id:4,
            title:"DifferenceType",
            content:"DifferenceType",
            description:'',
            tags:''
        },
        {
            url:'/XmlComparer/api/XmlComparer/DifferenceType',
            title:"DifferenceType",
            description:""
        }
    );
    var idx = lunr(function() {
        this.field('title');
        this.field('content');
        this.field('description');
        this.field('tags');
        this.ref('id');
        this.use(camelCaseTokenizer);

        this.pipeline.remove(lunr.stopWordFilter);
        this.pipeline.remove(lunr.stemmer);
        documents.forEach(function (doc) { this.add(doc) }, this)
    });

    return {
        search: function(q) {
            return idx.search(q).map(function(i) {
                return idMap[i.ref];
            });
        }
    };
}();
