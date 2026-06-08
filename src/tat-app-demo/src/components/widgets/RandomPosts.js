import React, { useState } from 'react'
import { useEffect } from 'react';
import PostEntry from '../blog/posts/PostEntry';

const RandomPosts = () => {
  const [randomPosts, setRandomPosts] = useState([]);

  useEffect(() => {
    loadRandomArticles();

    async function loadRandomArticles() {
      const response = await fetch('https://localhost:7085/api/posts/random/5');
      const data = await response.json();

      setRandomPosts(data.result);
    }
  }, []);

  return (
    <div className="mb-4">
      <h4 className='mb-3 text-danger'>
        Random Posts
      </h4>
      <div>
        {randomPosts.map((post) => (
          <PostEntry key={post.id} post={post} />
        ))}
      </div>
    </div>
  )
}

export default RandomPosts;