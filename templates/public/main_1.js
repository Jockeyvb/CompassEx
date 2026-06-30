export default {
  start: () => {
    // 1. 引入 Pagefind 样式
    const link = document.createElement('link');
    link.href = '/pagefind/pagefind-ui.css';
    link.rel = 'stylesheet';
    document.head.appendChild(link);

    // 2. 注入绝对优先级的强力 CSS 样式（用 !important 压制所有默认样式）
    const style = document.createElement('style');
    style.innerHTML = `
      /* 限制导航栏里搜索框的宽度，防止撑开导航栏 */
      #pagefind-search-box {
        --pagefind-ui-scale: 0.85;
        width: 100% !important;
      }
      /* 强制将结果面板从右侧剥离，变成全屏居中 800px 大弹窗 */
      #pagefind-search-box .pagefind-ui__drawer,
      .pagefind-ui__drawer {
        position: fixed !important;
        top: 75px !important;
        left: 50% !important;
        transform: translateX(-50%) !important; /* 强制屏幕水平正居中 */
        width: 800px !important; /* 锁定 800 像素 */
        max-width: 95vw !important;
        max-height: 75vh !important;
        overflow-y: auto !important;
        background: var(--bs-body-bg, #ffffff) !important;
        border: 1px solid var(--bs-border-color, #e0e0e0) !important;
        border-radius: 12px !important;
        box-shadow: 0 15px 40px rgba(0,0,0,0.15) !important;
        z-index: 99999 !important; /* 确保图层在最前 */
        padding: 25px !important;
        right: auto !important; /* 彻底清除之前右侧定位的干扰 */
      }
      /* 优化：没有任何输入或没有结果时，彻底隐藏大盒子，不挡住文章内容 */
      .pagefind-ui__drawer:empty {
        display: none !important;
      }
      /* 文本高亮 */
      mark {
        background-color: #fff3cd !important;
        color: #664d03 !important;
        padding: 1px 3px;
        border-radius: 3px;
        font-weight: bold;
      }
    `;
    document.head.appendChild(style);

    // 3. 异步加载 Pagefind 脚本并挂载
    const script = document.createElement('script');
    script.src = '/pagefind/pagefind-ui.js';
    script.onload = () => {
      const navContainer = document.querySelector('.navbar-nav') || document.querySelector('nav');
      
      if (navContainer) {
        // 创建外层包裹容器
        const wrapper = document.createElement('div');
        wrapper.style.cssText = 'width: 220px; margin-left: auto; margin-right: 15px; display: flex; align-items: center; position: relative;';
        
        const customSearchContainer = document.createElement('div');
        customSearchContainer.id = 'pagefind-search-box';
        
        wrapper.appendChild(customSearchContainer);
        navContainer.appendChild(wrapper);

        // 初始化 Pagefind
        new PagefindUI({ 
          element: "#pagefind-search-box", 
          showSubResults: true,
          resetStyles: false 
        });
      }
    };
    document.head.appendChild(script);
  }
}
