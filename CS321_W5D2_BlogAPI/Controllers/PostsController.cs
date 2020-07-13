using System;
using CS321_W5D2_BlogAPI.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CS321_W5D2_BlogAPI.Core.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CS321_W5D2_BlogAPI.Controllers
{
    //secure controller actions that change data
    [Route("api/[controller]")]
    public class PostsController : Controller
    {

        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        //get posts for blog
        //allow anyone to get, even if not logged in
        // GET /api/blogs/{blogId}/posts
        [HttpGet("/api/blogs/{blogId}/posts")]
        public IActionResult Get(int blogId)
        {
            try
            {
                var post = _postService.GetBlogPosts(blogId);

                if (post == null) return null;
                return Ok(post.ToApiModels());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("GetBlog", ex.Message);
                return BadRequest(ModelState);
            }
        }

        //get post by id
        //allow anyone to get, even if not logged in
        // GET api/blogs/{blogId}/posts/{postId}
        [HttpGet("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Get(int blogId, int postId)
        {
			try
			{
                var post = _postService.Get(postId);
                if (post == null) return null;
                return Ok(post.ToApiModel());
			}
			catch (Exception ex)
			{
                ModelState.AddModelError("GetBlogPost", ex.Message);
                return BadRequest(ModelState);
            }
        }

        //add a new post to blog
        // POST /api/blogs/{blogId}/post
        [HttpPost("/api/blogs/{blogId}/posts")]
        public IActionResult Post(int blogId, [FromBody]PostModel postModel)
        {
			try
			{
                var newPost = _postService.Add(postModel.ToDomainModel());
                return Ok(newPost);
			}
			catch (Exception ex)
			{
                ModelState.AddModelError("AddPost", ex.Message);
                return BadRequest(ModelState);
            }
        }

        // PUT /api/blogs/{blogId}/posts/{postId}
        [HttpPut("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Put(int blogId, int postId, [FromBody]PostModel postModel)
        {
            try
            {
                var updatedPost = _postService.Update(postModel.ToDomainModel());
                return Ok(updatedPost);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UpdatePost", ex.Message);
                return BadRequest(ModelState);
            }
        }

        //delete post by id
        // DELETE /api/blogs/{blogId}/posts/{postId}
        [HttpDelete("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Delete(int blogId, int postId)
        {
            try
			{
                _postService.Remove(postId);
                return Ok(_postService.Get(blogId));
			}
            catch (Exception ex)
			{
                ModelState.AddModelError("DeletePost", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
