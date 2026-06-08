import React from 'react';
// import FeaturedPosts from '../components/blog/posts/FeaturedPosts';
// import CandyList from '../components/candy/CandyList';
// import BestAuthors from '../components/widgets/BestAuthors';
import SubscriberList from '../components/widgets/SubscriberList';
import SubscribeForm from '../components/widgets/SubscriberForm';

const Home = () => {
  return (
    <div>
      <SubscribeForm />

      <SubscriberList />

      {/* <BestAuthors />
      <FeaturedPosts />
      <CandyList /> */}
    </div>
  )
}

export default Home;