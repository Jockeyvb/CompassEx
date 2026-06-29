call docfx metadata
call docfx build
call npx pagefind@1.2.0 --site _site
call docfx serve _site