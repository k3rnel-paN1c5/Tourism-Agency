import React from 'react';
import { Link } from 'react-router-dom';
import './PostCard.css';

const PostCard = ({ post }) => {
  return (
    <div className="post-card">
      <img src={post.image || 'https://via.placeholder.com/300'} alt={post.title} className="post-card-image" />
      <div className="post-card-content">
        <h3 className="post-card-title">{post.title}</h3>
        <p className="post-card-summary">{post.summary}</p>
        <Link to={`/posts/${post.id}`} className="post-card-read-more">Read More</Link>
      </div>
    </div>
  );
};

export default PostCard;