call docfx metadata
call docfx build
rem call npx pagefind@1.2.0 --site _site
call npx pagefind@1.2.0 --site _site --force-language zh-cn
call docfx serve _site