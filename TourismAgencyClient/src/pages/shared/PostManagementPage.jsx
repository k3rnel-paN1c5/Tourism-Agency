import React, { useState, useEffect } from 'react';
import { getAllPosts, getPostTypes } from '../../services/postService';
import DataTable from '../../components/shared/DataTable'; 
import Modal from '../../components/shared/Modal';
// You would also create a PostForm component for create/edit functionality
// import PostForm from '../../components/common/PostForm';
import DashboardHeader from '../../components/shared/DashboardHeader';

const PostManagementPage = () => {
  const [posts, setPosts] = useState([]);
  const [postTypes, setPostTypes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedPost, setSelectedPost] = useState(null);


  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const [postsResponse, postTypesResponse] = await Promise.all([
            getAllPosts(),
            getPostTypes()
        ]);
        setPosts(postsResponse.data);
        setPostTypes(postTypesResponse.data);
      } catch (err) {
        setError('Failed to fetch data.');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, []);

  const handleEdit = (post) => {
    setSelectedPost(post);
    setIsModalOpen(true);
  };
  
  const handleDelete = async (id) => {
      if(window.confirm('Are you sure you want to delete this post?')) {
          // call deletePost service
          // refresh data
      }
  };

  const columns = [
    { header: 'Title', accessor: 'title' },
    { header: 'Status', accessor: 'status' },
    { header: 'Views', accessor: 'views' },
    { header: 'Published Date', accessor: 'publishDate' },
  ];

  return (
    <div className="management-page">
      <DashboardHeader title="Post Management" />
      <button onClick={() => { setSelectedPost(null); setIsModalOpen(true); }}>
        Create New Post
      </button>
      
      {loading && <p>Loading...</p>}
      {error && <p className="error-message">{error}</p>}
      
      {!loading && !error && (
        <DataTable
          columns={columns}
          data={posts}
          onEdit={handleEdit}
          onDelete={handleDelete}
        />
      )}

      {isModalOpen && (
        <Modal onClose={() => setIsModalOpen(false)}>
            {/* You would render a <PostForm /> component here */}
            <h2>{selectedPost ? 'Edit Post' : 'Create Post'}</h2>
            <p>Post form would be here.</p>
        </Modal>
      )}
    </div>
  );
};

export default PostManagementPage;