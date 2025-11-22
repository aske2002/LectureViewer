from keybert import KeyBERT

doc = """
         Thank you.  Thank you.  And then we have the basis after this.  And then the heart is divided into the two halves.  So we have a left half over here and a right half over here.  And the entire division happens via septum cordis, which is the heart's sclera.  And then we have furthermore.  And then we have that division, that every half of the heart is also divided into two rooms.  And then we have this here, which is the right heart chamber.  And the name of a heart chamber is a ventriculus, which directly translated means a chamber.  We also have it in connection with the ventricles of the brain, which is also a chamber.  And we also have it in connection with the gastric sac, which is also called the ventricle.  And then this is ventriculus dexter, where this over here is sinister.  And then up here we have atrium.  And then we have a ventricle.
      """
kw_model = KeyBERT()
keywords = kw_model.extract_keywords(doc)

print(keywords)